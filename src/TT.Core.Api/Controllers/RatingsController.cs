using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using TT.Core.Application.Interfaces;
using TT.Core.Application.Dtos.Inputs;
using TT.Core.Application.Notifications;

namespace TT.Core.Api.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class RatingsController : MainController
{
    private readonly IRatingService _ratingService;

    public RatingsController(IRatingService ratingService,
        INotifier notifier) : base(notifier)
    {
        _ratingService = ratingService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateRating([FromForm] CreateRatingDto createRatingDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _ratingService.Create(createRatingDto);

        if (result.Equals(Guid.Empty))
            return BadRequest("");
        
        return Created(nameof(CreateRating), result);
    }
}
