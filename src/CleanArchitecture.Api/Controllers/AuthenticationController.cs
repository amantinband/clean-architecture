using CleanArchitecture.Application.Auth.Commands;
using CleanArchitecture.Application.Auth.Dto;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.Api.Controllers;

[Route("auth")]

public class AuthenticationController(ISender mediator) : ApiController
{
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> LoginAsync([FromBody] LoginCommand request)
    {
        var response = await mediator.Send(request);
        return Ok(response);
    }

    [HttpGet("test/api-key")]
    [Authorize(AuthenticationSchemes = "apiKey")]
    public IActionResult TestApiKey()
    {
        return NoContent();
    }

    [HttpGet("refresh-token")]
    public async Task<IActionResult> RefreshTokenAsync([FromQuery] string refreshToken, [FromQuery] Guid userId)
    {
        var command = new RefreshTokenCommand { RefreshToken = refreshToken, UserId = userId };
        var response = await mediator.Send(command);
        return Ok(response);
    }
}
