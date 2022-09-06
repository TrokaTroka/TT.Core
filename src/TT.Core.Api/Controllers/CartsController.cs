using Microsoft.AspNetCore.Mvc;

namespace TT.Core.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CartsController : ControllerBase
{
    public CartsController()
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
