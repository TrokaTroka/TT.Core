using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using TT.Core.Application.Interfaces;
using TT.Core.Application.Dtos.Inputs;

namespace TT.Core.Api.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class RatingsController : ControllerBase
{
    private readonly IRatingService _ratingService;

    public RatingsController(IRatingService ratingService)
    {
        _ratingService = ratingService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateRating([FromForm] CreateRatingDto createRatingDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var attempt = await _ratingService.Create(createRatingDto);

        if (attempt.Succeeded)
            return Created(nameof(CreateRating), attempt);

        return BadRequest(attempt);
    }
}
