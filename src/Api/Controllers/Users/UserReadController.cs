using API.Extensions;
using Application.Features.Collection.Queries.GetIsShrineInCollection;
using Application.Features.Collection.Queries.GetShrineCollectionCards;
using Application.Features.Collection.Queries.GetShrineCollectionIds;
using Application.Features.Users.Queries.GetMe;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
        var userId = User.GetUserId();

        var result = await _mediator.Send(new GetMeQuery(userId));
        return Ok(result);
    }

    // GET /api/users/me/collection/ids
    [HttpGet("me/collection/ids")]
    [Authorize]
    public async Task<ActionResult<GetShrineCollectionIdsResult>> GetShrineCollectionIdsAsync()
    {
        var userId = User.GetUserId();
        var result = await _mediator.Send(new GetShrineCollectionIdsQuery(userId));
        return Ok(result);
    }

    // GET /api/users/me/collection/cards?lat=...&lon=...
    [HttpGet("me/collection/cards")]
    [Authorize]
    public async Task<ActionResult<GetShrineCollectionCardsResult>> GetShrineCollectionCardsAsync(
        [FromQuery] double? lat,
        [FromQuery] double? lon
    )
    {
        var userId = User.GetUserId();
        var result = await _mediator.Send(new GetShrineCollectionCardsQuery(userId, lat, lon));
        return Ok(result);
    }

    // GET /api/users/me/collection/{shrineId}
    [HttpGet("me/collection/{shrineId}")]
    [Authorize]
    public async Task<ActionResult<GetIsShrineInCollectionResult>> IsShrineInCollectionAsync([FromRoute] int shrineId)
    {
        var userId = User.GetUserId();
        var result = await _mediator.Send(new GetIsShrineInCollectionQuery(userId, shrineId));
        return Ok(result);
    }
}
