using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TT.Core.Application.Dtos.Inputs;
using TT.Core.Application.Interfaces;

namespace TT.Core.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost]
    public async Task<IActionResult> UpdateUser([FromForm] UpdateUserDto updateUserDto)
    {
        var attempt = await _userService.UpdateUser(updateUserDto);

        if (attempt.Succeeded)
            return Ok(attempt);

        return BadRequest(attempt);
    }

    [AllowAnonymous]
    [HttpPost]
    [Route("activeaccount")]
    public async Task<IActionResult> ActiveAccount([FromQuery] Guid key)
    {
        var attempt = await _userService.ActiveAccount(key);

        if(attempt.Succeeded)
            return Ok(attempt);

        return BadRequest(attempt);
    }
}
