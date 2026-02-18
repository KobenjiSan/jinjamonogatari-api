using Application.Features.Users.Commands.LoginUser;
using Application.Features.Users.Commands.RegisterUser;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Users;

[ApiController]
[Route("api/users")]
public class UserWriteController : ControllerBase
{
    private readonly IMediator _mediator;

    public UserWriteController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // POST /api/users/login
    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync([FromBody] LoginUserRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Identifier))
            throw new ArgumentException("Username or Email is required.");

        if (string.IsNullOrWhiteSpace(request.Password))
            throw new ArgumentException("Password is required.");

        var result = await _mediator.Send(new LoginUserCommand(request.Identifier, request.Password));
        return Ok(result);
    }

    // POST /api/users/register
    [HttpPost("register")]
    public async Task<IActionResult> RegisterAsync([FromBody] RegisterUserRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Username))
            throw new ArgumentException("Username is required.");

        if (string.IsNullOrWhiteSpace(request.Email))
            throw new ArgumentException("Email is required.");

        if (string.IsNullOrWhiteSpace(request.Password))
            throw new ArgumentException("Password is required.");

        var result = await _mediator.Send(new RegisterUserCommand(
            request.Email,
            request.Username,
            request.Password));

         return Ok(result);
    }
}

// DTOs

public record LoginUserRequest
{
    public string Identifier { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
}

public record RegisterUserRequest
{
    public string Username { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
}
