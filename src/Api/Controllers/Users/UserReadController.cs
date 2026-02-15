using Application.Features.Users.Queries.GetMe;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Api.Controllers.Users;

[ApiController]
[Route("api/users")]
public class UserReadController : ControllerBase
{
    private readonly IMediator _mediator;

    public UserReadController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // GET /api/users/me
    [HttpGet("me")]
    [Authorize]
    public async Task<ActionResult<GetMeResult>> MeAsync()
    {
        var sub = User.FindFirstValue(ClaimTypes.NameIdentifier)
                  ?? User.FindFirstValue("sub");

        if (string.IsNullOrWhiteSpace(sub) || !int.TryParse(sub, out var userId))
            throw new UnauthorizedAccessException("Invalid token.");

        var result = await _mediator.Send(new GetMeQuery(userId));
        return Ok(result);
    }
}
