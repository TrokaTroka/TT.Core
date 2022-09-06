using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using TT.Core.Application.Interfaces;
using TT.Core.Application.Notifications;
using TT.Core.Application.Dtos.Inputs.Querys;

namespace TT.Core.Api.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class FavoritesController : MainController
{
    private readonly IFavoriteService _favoriteService;
    public FavoritesController(IFavoriteService favoriteService,
        INotifier notifier) : base(notifier)
    {
        _favoriteService = favoriteService;
    }

    [HttpGet]
    [Route("")]
    public async Task<IActionResult> GetMyFavorites([FromQuery] PaginationQuery paginationQuery)
    {
        var booksVM = await _favoriteService.GetMyFavorites(paginationQuery);

        if (booksVM == null)
            return NotFound("");

        return Ok(booksVM);
    }

    [HttpPost]
    [Route("{idBook}")]
    public async Task<IActionResult> CreateFavorite(Guid idBook)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        await _favoriteService.CreateFavorite(idBook);
                    
        return Created(nameof(CreateFavorite), idBook);
    }

    [HttpDelete]
    [Route("{idBook}")]
    public async Task<IActionResult> DeleteFavorite([FromRoute] Guid idBook)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        await _favoriteService.DeleteFavorite(idBook);

        return NoContent();
    }
}
