using System.Text.Json;
using Application.Features.Etiquette.Commands.CreateEtiquette;
using Application.Features.Etiquette.Commands.CreateStep;
using Application.Features.Etiquette.Commands.DeleteEtiquette;
using Application.Features.Etiquette.Commands.DeleteGlance;
using Application.Features.Etiquette.Commands.DeleteStep;
using Application.Features.Etiquette.Commands.UpdateEtiquette;
using Application.Features.Etiquette.Commands.UpdateGlance;
using Application.Features.Etiquette.Commands.UpdateStep;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Etiquette;

[ApiController]
[Route("api/etiquette")]
[Authorize(Roles = "Admin,Editor")]
public class EtiquetteWriteController : ControllerBase
{
    private readonly IMediator _mediator;

    public EtiquetteWriteController(IMediator mediator)
    {
        _mediator = mediator;
    }

    #region Create Etiquette

    // POST /api/etiquette/
    [HttpPost()]
    public async Task<IActionResult> CreateEtiquetteAsync([FromBody] CreateEtiquetteRequest request)
    {
        var command = new CreateEtiquetteCommand(request);
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    #endregion

    #region Update Etiquette

    // PUT /api/etiquette/{topicId}
    [HttpPut("{topicId}")]
    public async Task<IActionResult> UpdateEtiquetteAsync(
        [FromRoute] int topicId,
        [FromBody] UpdateEtiquetteRequest request
    )
    {
        var command = new UpdateEtiquetteCommand(topicId, request);
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    #endregion

    #region Delete Etiquette

    // DELETE /api/etiquette/{topicId}
    [HttpDelete("{topicId}")]
    public async Task<IActionResult> DeleteEtiquetteAsync([FromRoute] int topicId)
    {
        var command = new DeleteEtiquetteCommand(topicId);
        await _mediator.Send(command);
        return NoContent();
    }

    #endregion

    #region Create Step

    // POST /api/etiquette/steps/{topicId}
    [HttpPost("steps/{topicId}")]
    public async Task<IActionResult> CreateStepAsync(
        [FromRoute] int topicId, 
        [FromForm] string data,
        [FromForm] IFormFile? file
    )
    {
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        var request = JsonSerializer.Deserialize<CreateStepRequest>(data, options);

        if (request == null)
            return BadRequest("Invalid payload");

        var command = new CreateStepCommand(topicId, request, file);
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    #endregion

    #region Update Step

    // PUT /api/etiquette/steps/{stepId}
    [HttpPut("steps/{stepId}")]
    public async Task<IActionResult> UpdateStepAsync(
        [FromRoute] int stepId, 
        [FromForm] string data,
        [FromForm] IFormFile? file
    )
    {
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        var request = JsonSerializer.Deserialize<UpdateStepRequest>(data, options);

        if (request == null)
            return BadRequest("Invalid payload");

        var command = new UpdateStepCommand(stepId, request, file);
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    #endregion

    #region Delete Step

    // DELETE /api/etiquette/steps/{stepId}
    [HttpDelete("steps/{stepId}")]
    public async Task<IActionResult> DeleteStepAsync([FromRoute] int stepId)
    {
        var command = new DeleteStepCommand(stepId);
        await _mediator.Send(command);
        return NoContent();
    }

    #endregion

    #region Update Glance

    // PUT /api/etiquette/{topicId}/glance
    [HttpPut("{topicId}/glance")]
    public async Task<IActionResult> UpdateGlanceAsync(
        [FromRoute] int topicId,
        [FromBody] UpdateGlanceRequest request
    )
    {
        var command = new UpdateGlanceCommand(topicId, request);
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    #endregion

    #region Delete Glance

    // DELETE /api/etiquette/{topicId}/glance
    [HttpDelete("{topicId}/glance")]
    public async Task<IActionResult> DeleteGlanceAsync([FromRoute] int topicId)
    {
        var command = new DeleteGlanceCommand(topicId);
        await _mediator.Send(command);
        return NoContent();
    }

    #endregion
    
}

#region Etiquette Requests

public record CreateEtiquetteRequest(
    BasicEtiquetteRequest Basic,
    CitationCreateChangesRequest Citations
);

public record UpdateEtiquetteRequest(
    BasicEtiquetteRequest Basic,
    CitationListChangesRequest Citations
);

public record BasicEtiquetteRequest(
    string? Slug,
    string? TitleLong,
    string? Summary,
    bool ShowInGlance,
    bool ShowAsHighlight,
    int? GuideOrder
);

#endregion

#region Steps Requests

public record CreateStepRequest(
    BasicStepRequest Basic,
    CreateImageRequest? Image
);

public record UpdateStepRequest(
    BasicStepRequest Basic,
    ImageChangeRequest Image
);

public record BasicStepRequest(
    string? Text,
    int? StepOrder
);

#endregion

#region Glance Requests

public record UpdateGlanceRequest(
    string? TitleShort,
    string? IconKey,
    string? IconSet,
    int? GlanceOrder
);

#endregion