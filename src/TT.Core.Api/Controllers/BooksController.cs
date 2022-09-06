using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using TT.Core.Application.Dtos.Inputs.Querys;
using TT.Core.Application.Dtos.Inputs;
using TT.Core.Application.Interfaces;

namespace TT.Core.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BooksController : ControllerBase
{
    private readonly IBookService _bookService;
    public BooksController(IBookService bookService)
    {
        _bookService = bookService;
    }

    [HttpGet]
    [Route("")]
    public async Task<IActionResult> GetBookshell([FromQuery] PaginationQuery paginationQuery)
    {
        var attempt = await _bookService.GetBookshell(paginationQuery);

        if (attempt.Succeeded)
            return Ok(attempt);

        return NotFound(attempt);
    }

    [HttpGet]
    [Route("{idBook}")]
    public async Task<IActionResult> GetBookById(Guid idBook)
    {
        var attempt = await _bookService.GetBookById(idBook);

        if (attempt.Succeeded)
            return Ok(attempt);

        return BadRequest(attempt);
    }

    [HttpGet]
    [Route("my-books")]
    public async Task<IActionResult> GetMyBooks()
    {
        var attempt = await _bookService.GetMyBooks();

        if (attempt.Succeeded)
            return Ok(attempt);

        return BadRequest(attempt);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateBook([FromForm] CreateBookDto createBookDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (createBookDto.Images is null)
            return BadRequest();

        var attempt = await _bookService.CreateBook(createBookDto);

        if (attempt.Succeeded)
            return Created(nameof(GetBookById), attempt);

        return BadRequest(attempt);
    }

    [Authorize]
    [HttpPut]
    public async Task<IActionResult> UpdateBook([FromBody] UpdateBookDto updateBookDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var attempt = await _bookService.UpdateBook(updateBookDto);

        if(attempt.Succeeded)
            return Ok(attempt);

        return BadRequest(attempt);
    }

    [Authorize]
    [HttpDelete]
    [Route("{idBook}")]
    public async Task<IActionResult> DeleteBook([FromRoute] Guid idBook)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var attempt = await _bookService.DeleteBook(idBook);

        if (attempt.Succeeded)
            return Ok(attempt);

        return BadRequest(attempt);
    }
}
