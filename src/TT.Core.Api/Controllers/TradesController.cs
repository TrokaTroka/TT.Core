using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TT.Core.Application.Dtos.Inputs;
using TT.Core.Application.Interfaces;

namespace TT.Core.Api.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class TradesController : ControllerBase
{
    private readonly ITradeService _tradeService;
    public TradesController(ITradeService tradeService)
    {
        _tradeService = tradeService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllTrades()
    {
        var attempt = await _tradeService.GetAll();

        if (attempt.Succeeded)
            return Ok(attempt);

        return BadRequest(attempt);
    }

    [HttpPost]
    public async Task<IActionResult> Trade(CreateTradeDto tradeDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var attempt = await _tradeService.Create(tradeDto);

        if(attempt.Succeeded)
            return Created(nameof(GetAllTrades), attempt);

        return BadRequest(attempt);
    }
}
