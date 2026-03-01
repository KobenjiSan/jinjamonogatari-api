using API.Extensions;
using Application.Features.Collection.Commands.AddShrineToCollection;
using Application.Features.Collection.Commands.RemoveShrineFromCollection;
using Application.Features.Users.Commands.LoginUser;
using Application.Features.Users.Commands.RegisterUser;
using Application.Features.Users.Commands.RefreshTokens;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Application.Features.Users.Commands.LogoutUser;

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
    public async Task<ActionResult<LoginUserResult>> LoginAsync([FromBody] LoginUserRequest request)
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
    public async Task<ActionResult<RegisterUserResult>> RegisterAsync([FromBody] RegisterUserRequest request)
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

    // POST /api/users/refresh
    [HttpPost("refresh")]
    public async Task<ActionResult<RefreshTokenResult>> RefreshAsync([FromBody] RefreshTokenRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.RefreshToken))
            throw new ArgumentException("RefreshToken is required.");

        var result = await _mediator.Send(new RefreshTokenCommand(request.RefreshToken));
        return Ok(result);
    }

    // POST /api/users/logout
    [HttpPost("logout")]
    public async Task<IActionResult> LogoutAsync([FromBody] RefreshTokenRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.RefreshToken))
            throw new ArgumentException("RefreshToken is required.");

        await _mediator.Send(new LogoutUserCommand(request.RefreshToken));

        return NoContent();
    }

    // POST /api/users/me/collection/{shrineId}
    [HttpPost("me/collection/{shrineId}")]
    [Authorize]
    public async Task<IActionResult> AddShrineToCollectionAsync([FromRoute] int shrineId)
    {
        var userId = User.GetUserId();
        await _mediator.Send(new AddShrineToCollectionCommand(userId, shrineId));
        return NoContent();
    }

    // DELETE /api/users/me/collection/{shrineId}
    [HttpDelete("me/collection/{shrineId}")]
    [Authorize]
    public async Task<IActionResult> RemoveShrineFromCollectionAsync([FromRoute] int shrineId)
    {
        var userId = User.GetUserId();
        await _mediator.Send(new RemoveShrineFromCollectionCommand(userId, shrineId));
        return NoContent();
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

public record RefreshTokenRequest
{
    public string RefreshToken { get; init; } = string.Empty;
}
