using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using TT.Core.Application.Interfaces;
using TT.Core.Application.Dtos.Inputs.Querys;

namespace TT.Core.Api.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class FavoritesController : ControllerBase
{
    private readonly IFavoriteService _favoriteService;
    public FavoritesController(IFavoriteService favoriteService)
    {
        _favoriteService = favoriteService;
    }

    [HttpGet]
    [Route("")]
    public async Task<IActionResult> GetMyFavorites([FromQuery] PaginationQuery paginationQuery)
    {
        var attempt = await _favoriteService.GetMyFavorites(paginationQuery);

        if (attempt.Succeeded)
            return Ok(attempt);

        return NotFound(attempt);
    }

    [HttpPost]
    [Route("{idBook}")]
    public async Task<IActionResult> CreateFavorite(Guid idBook)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var attempt = await _favoriteService.CreateFavorite(idBook);
                 
        if(attempt.Succeeded)
            return Created(nameof(CreateFavorite), attempt.Success);

        return BadRequest(attempt);
    }

    [HttpDelete]
    [Route("{idBook}")]
    public async Task<IActionResult> DeleteFavorite([FromRoute] Guid idBook)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var attempt = await _favoriteService.DeleteFavorite(idBook);
        
        if(attempt.Succeeded)
            return Ok(attempt);

        return BadRequest(attempt);
    }
}
