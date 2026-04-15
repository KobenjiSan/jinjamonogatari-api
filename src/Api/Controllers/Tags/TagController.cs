using Application.Features.Shrines.Models;
using Application.Features.Tags.Commands.CreateTag;
using Application.Features.Tags.Commands.DeleteTag;
using Application.Features.Tags.Commands.UpdateTag;
using Application.Features.Tags.Queries.GetAllTagsDropdown;
using Application.Features.Tags.Queries.GetAllTagsListCMS;
using Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/tags")]
public class TagController : ControllerBase
{
    private readonly IMediator _mediator;

    public TagController(IMediator mediator)
    {
        _mediator = mediator;
    }

    #region Tags List

    // Returns all tags
    // GET /api/tags/cms/tags?page=...&pageSize=...&searchQuery=...&sort=...
    [HttpGet("cms/tags")]
    [Authorize]
    public async Task<ActionResult<GetAllTagsListCMSResult>> GetAllTagsCMSAsync(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 5,
        [FromQuery] string? searchQuery = null,
        [FromQuery] TagsSort? sort = null
    )
    {
        var result = await _mediator.Send(new GetAllTagsListCMSQuery(searchQuery, sort, page, pageSize));
        return Ok(result);
    }

    #endregion

    #region Tags Dropdown

    // Returns all tags
    // GET /api/tags/cms/dropdown
    [HttpGet("cms/dropdown")]
    [Authorize]
    public async Task<ActionResult<IReadOnlyList<TagDto>>> GetAllTagsDropdownAsync()
    {
        var result = await _mediator.Send(new GetAllTagsDropdownQuery());
        return Ok(result.Tags);
    }

    #endregion

    #region Create Tag

    // POST /api/tags/
    [HttpPost()]
    [Authorize]
    public async Task<IActionResult> CreateTagAsync([FromBody] TagRequest request)
    {
        await _mediator.Send(new CreateTagCommand(request));
        return NoContent();
    }

    #endregion

    #region Update Tag

    // PUT /api/tags/{tagId}
    [HttpPut("{tagId}")]
    [Authorize]
    public async Task<IActionResult> UpdateTagAsync([FromRoute] int tagId, [FromBody] TagRequest request)
    {
        await _mediator.Send(new UpdateTagCommand(tagId, request));
        return NoContent();
    }

    #endregion

    #region Delete Tag

    // DELETE /api/tags/{tagId}
    [HttpDelete("{tagId}")]
    [Authorize]
    public async Task<IActionResult> DeleteTagAsync([FromRoute] int tagId)
    {
        await _mediator.Send(new DeleteTagCommand(tagId));
        return NoContent();
    }

    #endregion
}

#region Requests

public record TagRequest(
    string TitleEn,
    string? TitleJp
);

#endregion