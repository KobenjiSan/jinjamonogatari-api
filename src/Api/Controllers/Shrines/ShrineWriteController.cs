using Application.Features.Shrines.Commands.UpdateShrineMeta;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/shrines")]
public class ShrineWriteController : ControllerBase
{
    private readonly IMediator _mediator;

    public ShrineWriteController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // Meta
    // PUT /api/shrines/cms/{id}/meta
    [HttpPut("cms/{id}/meta")]
    [Authorize]
    public async Task<IActionResult> UpdateShrineMeta(
        [FromRoute] int id,
        [FromBody] UpdateShrineMetaRequest request
    )
    {
        var command = new UpdateShrineMetaCommand(id, request);
        var result = await _mediator.Send(command);
        return Ok(result);
    }
}

// Request DTOs

// Meta
public record UpdateShrineMetaRequest(
    BasicMetaUpdateRequest Basic,
    TagChangesRequest Tags,
    HeroImageChangeRequest HeroImage
);

public record BasicMetaUpdateRequest(
    string? Slug,
    string? NameEn,
    string? NameJp,
    string? ShrineDesc,
    decimal? Lat,
    decimal? Lon,
    string? Prefecture,
    string? City,
    string? Ward,
    string? Locality,
    string? PostalCode,
    string? Country,
    string? PhoneNumber,
    string? Email,
    string? Website
);

// Tag Changes
public record TagChangesRequest(
    IReadOnlyList<CreateTagRequest> Create,
    IReadOnlyList<UpdateTagRequest> Update,
    IReadOnlyList<int> Delete
);
public record CreateTagRequest(
    string TitleEn,
    string? TitleJp
);
public record UpdateTagRequest(
    int TagId,
    string TitleEn,
    string? TitleJp
);

// Hero Image Changes
public record HeroImageChangeRequest(
    string Action,
    string? ImgSource,
    string? Title,
    string? Desc,
    HeroImageCitationRequest? Citation
);
public record HeroImageCitationRequest(
    string? Title,
    string? Author,
    string? Url,
    int? Year
);