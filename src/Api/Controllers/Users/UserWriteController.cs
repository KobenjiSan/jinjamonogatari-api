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
using Application.Features.Users.Commands.UpdateMyProfile;
using System.Text.Json;
using Application.Features.Users.Commands.LoginUserCMS;
using Application.Features.Users.Commands.DeleteUser;
using Application.Common.Exceptions;
using Application.Features.Users.Commands.UpdateUserRole;

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

    // POST /api/users/login/cms
    [HttpPost("login/cms")]
    public async Task<ActionResult<LoginUserCMSResult>> LoginCMSAsync([FromBody] LoginUserRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Identifier))
            throw new ArgumentException("Username or Email is required.");

        if (string.IsNullOrWhiteSpace(request.Password))
            throw new ArgumentException("Password is required.");

        var result = await _mediator.Send(new LoginUserCMSCommand(request.Identifier, request.Password));
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

    // PUT /api/users/me/profile
    [HttpPut("me/profile")]
    [Authorize]
    public async Task<ActionResult<UpdateMyProfileResult>> UpdateMyProfileAsync(
        [FromBody] UpdateMyProfileRequest request)
    {
        var userId = User.GetUserId();

        bool hasFirstName = request.FirstName.HasValue;
        bool hasLastName = request.LastName.HasValue;
        bool hasPhone = request.Phone.HasValue;

        string? firstName = request.FirstName is { } fn ? fn.ToString() : null;
        string? lastName  = request.LastName  is { } ln ? ln.ToString() : null;
        string? phone     = request.Phone     is { } ph ? ph.ToString() : null;

        firstName = string.IsNullOrWhiteSpace(firstName) ? "" : firstName;
        lastName  = string.IsNullOrWhiteSpace(lastName)  ? "" : lastName;
        phone     = string.IsNullOrWhiteSpace(phone)     ? "" : phone;

        var result = await _mediator.Send(new UpdateMyProfileCommand(
            userId,
            hasFirstName, firstName,
            hasLastName, lastName,
            hasPhone, phone
        ));

        return Ok(result);
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

    // DELETE /api/users/admin/{userId}
    [HttpDelete("admin/{userId}")]
    [Authorize(Roles = "Admin")]    // Admins only
    public async Task<IActionResult> DeleteUserAsync([FromRoute] int userId)
    {
        await _mediator.Send(new DeleteUserCommand(userId));
        return NoContent();
    }

    // PUT /api/users/admin/{userId}
    [HttpPut("admin/{userId}")]
    [Authorize(Roles = "Admin")]    // Admins only
    public async Task<IActionResult> UpdateUserRoleAsync([FromRoute] int userId, [FromBody] UpdateUserRoleRequest request)
    {
        if(string.IsNullOrWhiteSpace(request.userRole)) throw new BadRequestException("User role is required.");
        await _mediator.Send(new UpdateUserRoleCommand(userId, request.userRole));
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

public record UpdateMyProfileRequest
{
    public JsonElement? FirstName { get; init; }
    public JsonElement? LastName { get; init; }
    public JsonElement? Phone { get; init; }
}

public record UpdateUserRoleRequest(
    string userRole
);
