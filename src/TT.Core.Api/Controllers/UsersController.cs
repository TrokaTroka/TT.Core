using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TT.Core.Application.Dtos.Inputs;
using TT.Core.Application.Interfaces;
using TT.Core.Application.Notifications;

namespace TT.Core.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class UsersController : MainController
{
    private readonly IUserService _userService;
    public UsersController(IUserService userService,
        INotifier notifier) : base(notifier)
    {
        _userService = userService;
    }

    [HttpPost]
    public async Task<IActionResult> UpdateUser([FromForm] UpdateUserDto updateUserDto)
    {
        var result = await _userService.UpdateUser(updateUserDto);

        if (result == null)
            return BadRequest("");

        return Ok(result);
    }

    [AllowAnonymous]
    [HttpPost]
    [Route("activeaccount")]
    public async Task<IActionResult> ActiveAccount([FromQuery] Guid key)
    {
        await _userService.ActiveAccount(key);

        return Ok();
    }
}
