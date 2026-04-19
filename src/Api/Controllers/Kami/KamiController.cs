using System.Text.Json;
using Application.Features.Kami.Commands.CreateKami;
using Application.Features.Kami.Commands.DeleteKami;
using Application.Features.Kami.Commands.UpdateKami;
using Application.Features.Kami.Queries.GetAllKamiCMS;
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
    public async Task<IActionResult> UpdateKamiAsync(
        [FromRoute] int kamiId,
        [FromForm] string data,
        [FromForm] IFormFile? file
    )
    {
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        var request = JsonSerializer.Deserialize<UpdateKamiRequest>(data, options);

        if (request == null)
            return BadRequest("Invalid payload");

        var command = new UpdateKamiCommand(kamiId, request, file);
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    #endregion

    #region Delete Kami

    // DELETE /api/kami/{kamiId}
    [HttpDelete("{kamiId}")]
    public async Task<IActionResult> DeleteKamiAsync([FromRoute] int kamiId)
    {
        await _mediator.Send(new DeleteKamiCommand(kamiId));
        return NoContent();
    }

    #endregion
}