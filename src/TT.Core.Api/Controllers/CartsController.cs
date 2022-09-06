using Microsoft.AspNetCore.Mvc;
using TT.Core.Application.Notifications;

namespace TT.Core.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CartsController : MainController
{
    public CartsController(INotifier notifier) : base(notifier)
    { }

    [HttpGet]
    public async Task<IActionResult> GetBooksCart()
    {
        return Ok();
    }

    [HttpPost("addCart")]
    public async Task<IActionResult> AddBookCart()
    {
        return Created(nameof(GetBooksCart),"");
    }

    [HttpDelete("deleteCart")]
    public async Task<IActionResult> DeleteBookCart()
    {
        return NoContent();
    }
}
