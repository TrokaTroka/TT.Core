using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TT.Core.Application.Dtos.Inputs;
using TT.Core.Application.Interfaces;
using TT.Core.Application.Notifications;

namespace TT.Core.Api.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class TradesController : MainController
{
    private readonly ITradeService _tradeService;
    public TradesController(ITradeService tradeService,
        INotifier notifier) : base(notifier)
    {
        _tradeService = tradeService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllTrades()
    {
        var result = await _tradeService.GetAll();

        if (result is null)
            return BadRequest("");

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Trade(CreateTradeDto tradeDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        await _tradeService.Create(tradeDto);

        return Created(nameof(GetAllTrades), "");
    }
}
