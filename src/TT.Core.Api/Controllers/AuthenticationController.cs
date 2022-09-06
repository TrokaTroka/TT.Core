using Microsoft.AspNetCore.Mvc;
using TT.Core.Application.Dtos.Inputs;
using TT.Core.Application.Interfaces;

namespace TT.Core.Api.Controllers;

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
        return new JsonResult("Api okay");
    }

    [HttpPost]
    [Route("signin")]
    public async Task<IActionResult> SignIn([FromBody] LoginDto inputDto)
    {
        var attempt = await _authService.AuthenticateUser(inputDto);

        if (attempt.Succeeded)
            return Ok(attempt);

        return BadRequest(attempt);        
    }

    [HttpPost]
    [Route("signup")]
    public async Task<IActionResult> SignUp([FromBody] CreateUserDto inputDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var attempt = await _authService.CreateUser(inputDto);

        if (attempt.Succeeded)
            return Created(nameof(SignIn), attempt);

        return BadRequest(attempt);
    }

    [HttpPost]
    [Route("refresh-token/{refreshTokenId}")]
    public async Task<IActionResult> RefreshToken(string refreshTokenId)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var attemptRefreshToken = await _authService.GetRefreshToken(Guid.Parse(refreshTokenId));

        if (!attemptRefreshToken.Succeeded)
            return BadRequest(attemptRefreshToken.Failure.Message);

        var attempt = await _authService.GenerateToken(attemptRefreshToken.Success);

        if (attempt.Succeeded)
            return Ok(attempt);

        return BadRequest(attempt);
    }

    [HttpPost]
    [Route("resetpassword")]
    public async Task<IActionResult> SendLinkResetPassord(SendLinkResetPasswordDto inputDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var attempt = await _authService.SendResetPasswordLink(inputDto);

        if (attempt.Succeeded)
            return Ok(attempt);

        return BadRequest(attempt);
    }

    [HttpPut]
    [Route("resetpassword")]
    public async Task<IActionResult> ResetPassord(ResetPasswordDto inputDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var attempt = await _authService.ResetPassword(inputDto);

        if (attempt.Succeeded)
            return Ok(attempt);

        return BadRequest(attempt);
    }
}
