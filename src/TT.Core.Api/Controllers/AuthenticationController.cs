using Microsoft.AspNetCore.Mvc;
using TT.Authentication.Application.Dtos.Inputs;
using TT.Authentication.Application.Interfaces;
using TT.Package.Core.Controllers;

namespace TT.Authentication.Api.Controllers;

[Route("api/authentication")]
[ApiController]
public class AuthenticationController : ControllerBase
{
    private readonly IAuthenticateService _authService;
    public AuthenticationController(IAuthenticateService authService)
    {
        _authService = authService;
    }

    [HttpGet]
    [Route("check")]
    public IActionResult Check()
    {
        return new JsonResult("Api ok");
    }

    [HttpPost]
    [Route("signin")]
    public async Task<IActionResult> SignIn([FromBody] LoginDto inputDto)
    {
        var result = await _authService.AuthenticateUser(inputDto);

        if (!result.Succeeded)            
            return BadRequest(result.Failure.Message);
        
        return Ok(result.Success);
    }

    [HttpPost]
    [Route("signup")]
    public async Task<IActionResult> SignUp([FromBody] CreateUserDto inputDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _authService.CreateUser(inputDto);

        if (!result.Succeeded)
            return BadRequest(result.Failure.Message);

        return Created(nameof(SignIn), result.Success);
    }

    [HttpPost]
    [Route("refresh-token/{refreshTokenId}")]
    public async Task<IActionResult> RefreshToken(string refreshTokenId)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var refreshToken = await _authService.GetRefreshToken(Guid.Parse(refreshTokenId));

        if (!refreshToken.Succeeded)
            return BadRequest(refreshToken.Failure.Message);

        var result = await _authService.GenerateToken(refreshToken.Success);

        if (result.Succeeded)
            return BadRequest(result.Failure.Message);

        return Ok(result.Success);
    }

    [HttpPost]
    [Route("resetpassword")]
    public async Task<IActionResult> SendLinkResetPassord(SendLinkResetPasswordDto inputDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _authService.SendResetPasswordLink(inputDto);

        if (result.Succeeded)
            return BadRequest(result.Failure.Message);

        return Ok();
    }

    [HttpPut]
    [Route("resetpassword")]
    public async Task<IActionResult> ResetPassord(ResetPasswordDto inputDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _authService.ResetPassword(inputDto);

        if (result.Succeeded)
            return BadRequest(result.Failure.Message);

        return Ok();
    }
}
