using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using TT.Core.Application.Dtos.Inputs.Querys;
using TT.Core.Application.Dtos.Inputs;
using TT.Core.Application.Interfaces;
using TT.Core.Application.Notifications;

namespace TT.Core.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BooksController : MainController
{
    private readonly IBookService _bookService;
    public BooksController(IBookService bookService,
        INotifier notifier) : base(notifier)
    {
        _bookService = bookService;
    }

    [HttpGet]
    [Route("")]
    public async Task<IActionResult> GetBookshell([FromQuery] PaginationQuery paginationQuery)
    {
        var booksVM = await _bookService.GetBookshell(paginationQuery);

        if (booksVM is null)
            return NotFound("");

        return Ok(booksVM);
    }

    [HttpGet]
    [Route("{idBook}")]
    public async Task<IActionResult> GetBookById(Guid idBook)
    {
        var bookVM = await _bookService.GetBookById(idBook);

        if (bookVM is null)
            return BadRequest();

        return Ok(bookVM);
    }

    [HttpGet]
    [Route("my-books")]
    public async Task<IActionResult> GetMyBooks()
    {
        var bookVM = await _bookService.GetMyBooks();

        if (bookVM is null)
            return BadRequest("");

        return Ok(bookVM);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateBook([FromForm] CreateBookDto createBookDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (createBookDto.Images is null)
            return BadRequest();

        var result = await _bookService.CreateBook(createBookDto);

        if (result.Equals(Guid.Empty))
            return BadRequest();
        
        return Created(nameof(GetBookById), result);
    }

    [Authorize]
    [HttpPut]
    public async Task<IActionResult> UpdateBook([FromBody] UpdateBookDto updateBookDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        await _bookService.UpdateBook(updateBookDto);

        return NoContent();
    }

    [Authorize]
    [HttpDelete]
    [Route("{idBook}")]
    public async Task<IActionResult> DeleteBook([FromRoute] Guid idBook)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        await _bookService.DeleteBook(idBook);

        return NoContent();
    }
}
