using Microsoft.AspNetCore.Mvc;
using TT.Core.Application.Dtos.Inputs;
using TT.Core.Application.Interfaces;
using TT.Core.Application.Notifications;

namespace TT.Core.Api.Controllers;

[Route("api/authentication")]
[ApiController]
public class AuthenticationController : MainController
{
    private readonly IAuthenticateService _authService;
    public AuthenticationController(IAuthenticateService authService,
        INotifier notifier) : base(notifier)
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
        var attempt = await _authService.AuthenticateUser(inputDto);

        if (!attempt.Succeeded)            
            return BadRequest(attempt.Failure.Message);
        
        return Ok(attempt.Success);
    }

    [HttpPost]
    [Route("signup")]
    public async Task<IActionResult> SignUp([FromBody] CreateUserDto inputDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var attempt = await _authService.CreateUser(inputDto);

        if (!attempt.Succeeded)
            return BadRequest(attempt.Failure.Message);

        return Created(nameof(SignIn), attempt.Success);
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
            return BadRequest(attempt.Failure.Message);

        return Ok(attempt.Success);
    }

    [HttpPost]
    [Route("resetpassword")]
    public async Task<IActionResult> SendLinkResetPassord(SendLinkResetPasswordDto inputDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var attempt = await _authService.SendResetPasswordLink(inputDto);

        if (attempt.Succeeded)
            return BadRequest(attempt.Failure.Message);

        return Ok(attempt.Success);
    }

    [HttpPut]
    [Route("resetpassword")]
    public async Task<IActionResult> ResetPassord(ResetPasswordDto inputDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var attempt = await _authService.ResetPassword(inputDto);

        if (attempt.Succeeded)
            return BadRequest(attempt.Failure.Message);

        return Ok(attempt.Success);
    }
}
