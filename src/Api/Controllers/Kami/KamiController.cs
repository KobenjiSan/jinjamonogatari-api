using System.Text.Json;
using API.Extensions;
using Application.Features.Kami.Commands.CreateKami;
using Application.Features.Kami.Commands.DeleteKami;
using Application.Features.Kami.Commands.PublishReviewKami;
using Application.Features.Kami.Commands.RejectReviewKami;
using Application.Features.Kami.Commands.SubmitReviewKami;
using Application.Features.Kami.Commands.UpdateKami;
using Application.Features.Kami.Models;
using Application.Features.Kami.Queries.GetAllKamiCMS;
using Application.Features.Kami.Queries.GetKamiReviewHistory;
using Application.Features.Shrines.Models;
using Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/kami")]
[Authorize]
public class KamiController : ControllerBase
{
    private readonly IMediator _mediator;

    public KamiController(IMediator mediator)
    {
        _mediator = mediator;
    }

    #region Kami List

    // GET /api/kami/cms/kami?page=...&pageSize=...&searchQuery=...&sort=...
    [HttpGet("cms/kami")]
    public async Task<ActionResult<GetAllKamiCMSResult>> GetAllKamiCMSAsync(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 5,
        [FromQuery] string? searchQuery = null,
        [FromQuery] TagsSort? sort = null
    )
    {
        var result = await _mediator.Send(new GetAllKamiCMSQuery(searchQuery, sort, page, pageSize));
        return Ok(result);
    }

    #endregion

    #region Create Kami

    // POST /api/kami/
    [HttpPost()]
    public async Task<IActionResult> CreateKamiAsync([FromForm] string data, [FromForm] IFormFile? file)
    {
        User.EnsureNotDemo();
        
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        var request = JsonSerializer.Deserialize<CreateKamiInShrineRequest>(data, options);

        if (request == null)
            return BadRequest("Invalid payload");

        var command = new CreateKamiCommand(request, file);
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    #endregion

    #region Update Kami

    // PUT /api/kami/{kamiId}
    [HttpPut("{kamiId}")]
    public async Task<ActionResult<KamiReadCMSDto>> UpdateKamiAsync(
        [FromRoute] int kamiId,
        [FromForm] string data,
        [FromForm] IFormFile? file
    )
    {
        User.EnsureNotDemo();

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        var request = JsonSerializer.Deserialize<UpdateKamiRequest>(data, options);

        if (request == null)
            return BadRequest("Invalid payload");

        var command = new UpdateKamiCommand(kamiId, request, file);
        var result = await _mediator.Send(command);
        return Ok(result.Kami);
    }

    #endregion

    #region Delete Kami

    // DELETE /api/kami/{kamiId}
    [HttpDelete("{kamiId}")]
    public async Task<IActionResult> DeleteKamiAsync([FromRoute] int kamiId)
    {
        User.EnsureNotDemo();
        await _mediator.Send(new DeleteKamiCommand(kamiId));
        return NoContent();
    }

    #endregion

    #region SUBMIT REVIEW

    // POST /api/kami/{kamiId}/review/submit
    [HttpPost("{kamiId}/review/submit")]
    public async Task<IActionResult> SubmitReviewKamiAsync([FromRoute] int kamiId)
    {
        User.EnsureNotDemo();
        var userId = User.GetUserId();
        var username = User.GetEmail();
        var command = new SubmitReviewKamiCommand(username, kamiId, userId);
        await _mediator.Send(command);
        return NoContent();
    }

    #endregion

    #region REJECT REVIEW KAMI

    // POST /api/kami/{kamiId}/review/reject
    [HttpPost("{kamiId}/review/reject")]
    [Authorize(Roles = "Admin")]    // Admins only
    public async Task<IActionResult> RejectReviewKamiAsync([FromRoute] int kamiId, [FromBody] RejectKamiRequest request)
    {
        User.EnsureNotDemo();
        var userId = User.GetUserId();
        var username = User.GetEmail();
        var command = new RejectReviewKamiCommand(username, kamiId, userId, request.Message);
        await _mediator.Send(command);
        return NoContent();
    }

    #endregion

    #region PUBLISH REVIEW KAMI

    // POST /api/kami/{kamiId}/review/publish
    [HttpPost("{kamiId}/review/publish")]
    [Authorize(Roles = "Admin")]    // Admins only
    public async Task<IActionResult> PublishReviewKamiAsync([FromRoute] int kamiId)
    {
        User.EnsureNotDemo();
        var userId = User.GetUserId();
        var username = User.GetEmail();
        var command = new PublishReviewKamiCommand(username, kamiId, userId);
        await _mediator.Send(command);
        return NoContent();
    }

    #endregion

     #region GET KAMI REVIEW HISTORY

    // POST /api/kami/{kamiId}/review/history
    [Authorize]
    [HttpGet("{kamiId}/review/history")]
    public async Task<ActionResult<IReadOnlyList<KamiReviewDto>>> GetKamiReviewHistoryAsync([FromRoute] int kamiId)
    {
        var result = await _mediator.Send(new GetKamiReviewHistoryQuery(kamiId));
        return Ok(result.ReviewHistory);
    }

    #endregion
}

#region Reject Kami Request

public record RejectKamiRequest(string Message);

#endregion