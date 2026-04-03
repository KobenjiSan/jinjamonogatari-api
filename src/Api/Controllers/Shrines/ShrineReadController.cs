using Application.Common.Models.Citations;
using Application.Common.Models.Images;
using Application.Features.Shrines.Models;
using Application.Features.Shrines.Queries.GetAllKamiListCMS;
using Application.Features.Shrines.Queries.GetAllTagsListCMS;
using Application.Features.Shrines.Queries.GetImageById;
using Application.Features.Shrines.Queries.GetImportPreviewCMS;
using Application.Features.Shrines.Queries.GetShrineAudit;
using Application.Features.Shrines.Queries.GetShrineCitationsByIdCMS;
using Application.Features.Shrines.Queries.GetShrineCitationsDropdownByIdCMS;
using Application.Features.Shrines.Queries.GetShrineFolkloreByIdCMS;
using Application.Features.Shrines.Queries.GetShrineFolkloreBySlug;
using Application.Features.Shrines.Queries.GetShrineGalleryByIdCMS;
using Application.Features.Shrines.Queries.GetShrineGalleryBySlug;
using Application.Features.Shrines.Queries.GetShrineHistoryByIdCMS;
using Application.Features.Shrines.Queries.GetShrineHistoryBySlug;
using Application.Features.Shrines.Queries.GetShrineKamiByIdCMS;
using Application.Features.Shrines.Queries.GetShrineKamiBySlug;
using Application.Features.Shrines.Queries.GetShrineListCMS;
using Application.Features.Shrines.Queries.GetShrineListView;
using Application.Features.Shrines.Queries.GetShrineMapPoints;
using Application.Features.Shrines.Queries.GetShrineMetaByIdCMS;
using Application.Features.Shrines.Queries.GetShrineMetaBySlug;
using Application.Features.Shrines.Queries.GetShrineNotesByIdCMS;
using Application.Features.Shrines.Queries.GetShrinePreview;
using Application.Features.Shrines.Queries.GetShrineStatusByIdCMS;
using Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/shrines")]
public class ShrineReadController : ControllerBase
{
    private readonly IMediator _mediator;

    public ShrineReadController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // Endpoints needed

    // Map Screen - shrine points on map (list of ShrineMapPointDto) [will (possibly) need PFS later and/or bounding box]
    // GET /api/shrines/map
    [HttpGet("map")]
    public async Task<ActionResult<IReadOnlyList<ShrineMapPointDto>>> GetShrineMapPointsAsync()
    {
        var result = await _mediator.Send(new GetShrineMapPointsQuery());
        return Ok(result.MapPoints);
    }

    // Map Screen - shrine preview (ShrinePreviewDto)
    // GET /api/shrines/map/{slug}?lat=...&lon=...
    [HttpGet("map/{slug}")]
    public async Task<ActionResult<ShrinePreviewDto>> GetShrinePreviewAsync(
        [FromRoute] string slug,
        [FromQuery] double? lat,
        [FromQuery] double? lon
    )
    {
        var result = await _mediator.Send(new GetShrinePreviewQuery(slug, lat, lon));
        return Ok(result.Preview);
    }

    // List Screen - (list of ShrineCardDto) [will need PFS later]
    // GET /api/shrines/list-view?lat=...&lon=...
    [HttpGet("list-view")]
    public async Task<ActionResult<IReadOnlyList<ShrineCardDto>>> GetShrineListViewAsync(
        [FromQuery] double? lat,
        [FromQuery] double? lon,
        [FromQuery] string? q
    )
    {
        var result = await _mediator.Send(new GetShrineListViewQuery(lat, lon, q));
        return Ok(result.Shrines);
    }

    // Shrine Screens
    // Main page / hero - inital load (ShrineMetaDto)
    // GET /api/shrines/{slug}/meta?lat=...&lon=...
    [HttpGet("{slug}/meta")]
    public async Task<ActionResult<ShrineMetaDto>> GetShrineMetaBySlugAsync(
        [FromRoute] string slug,
        [FromQuery] double? lat,
        [FromQuery] double? lon
    )
    {
        var result = await _mediator.Send(new GetShrineMetaBySlugQuery(slug, lat, lon));
        return Ok(result.Meta);
    }

    // Kami page (list of KamiReadDto)
    // GET /api/shrines/{slug}/kami
    [HttpGet("{slug}/kami")]
    public async Task<ActionResult<IReadOnlyList<KamiReadDto>>> GetShrineKamiBySlugAsync([FromRoute] string slug)
    {
        var result = await _mediator.Send(new GetShrineKamiBySlugQuery(slug));
        return Ok(result.Kami);
    }

    // History page (list of HistoryReadDto)
    // GET /api/shrines/{slug}/history
    [HttpGet("{slug}/history")]
    public async Task<ActionResult<IReadOnlyList<HistoryReadDto>>> GetShrineHistoryBySlugAsync([FromRoute] string slug)
    {
        var result = await _mediator.Send(new GetShrineHistoryBySlugQuery(slug));
        return Ok(result.History);
    }

    // folklore page (list of FolkloreReadDto)
    // GET /api/shrines/{slug}/folklore
    [HttpGet("{slug}/folklore")]
    public async Task<ActionResult<IReadOnlyList<FolkloreReadDto>>> GetShrineFolkloreBySlugAsync([FromRoute] string slug)
    {
        var result = await _mediator.Send(new GetShrineFolkloreBySlugQuery(slug));
        return Ok(result.Folklore);
    }

    // gallery page (GalleryListItemDto)
    // GET /api/shrines/{slug}/gallery
    [HttpGet("{slug}/gallery")]
    public async Task<ActionResult<IReadOnlyList<ImageCitedDto>>> GetShrineGalleryBySlugAsync([FromRoute] string slug)
    {
        var result = await _mediator.Send(new GetShrineGalleryBySlugQuery(slug));
        return Ok(result.Images);
    }

    // TEMP UNTIL I NEED FULL IMAGE CONTROLLER
    // gallery item clicked (ImageFullDto)
    // GET /api/shrines/image/{id} 
    [HttpGet("image/{id}")]
    public async Task<ActionResult<ImageFullDto>> GetImageByIdAsync([FromRoute] int id)
    {
        var result = await _mediator.Send(new GetImageByIdQuery(id));
        return Ok(result.Image);
    }

    // =======
    // CMS
    // =======

    // GET /api/shrines/cms/list?status=...&prefecture=...&searchQuery=...&sort=...&page=...&pageSize=...
    [HttpGet("cms/list")]
    [Authorize]
    public async Task<ActionResult<GetShrineListCMSResult>> GetShrineListCMSAsync(
        [FromQuery] string? status,
        [FromQuery] string? prefecture = null,
        [FromQuery] string? searchQuery = null,
        [FromQuery] ShrineSort? sort = null,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 5
    )
    {
        var result = await _mediator.Send(new GetShrineListCMSQuery(status, prefecture, searchQuery, sort, page, pageSize));
        return Ok(result);
    }

    // GET /api/shrines/cms/{id}/meta
    [HttpGet("cms/{id}/meta")]
    [Authorize]
    public async Task<ActionResult<ShrineMetaCMSDto>> GetShrineMetaByIdCMSAsync([FromRoute] int id)
    {
        var result = await _mediator.Send(new GetShrineMetaByIdCMSQuery(id));
        return Ok(result.Meta);
    }

    // GET /api/shrines/cms/{id}/notes
    [HttpGet("cms/{id}/notes")]
    [Authorize]
    public async Task<ActionResult<string>> GetShrineNotesByIdCMSAsync([FromRoute] int id)
    {
        var result = await _mediator.Send(new GetShrineNotesByIdCMSQuery(id));
        return Ok(result.Notes);
    }

    // GET /api/shrines/cms/{id}/kami
    [HttpGet("cms/{id}/kami")]
    [Authorize]
    public async Task<ActionResult<IReadOnlyList<KamiReadCMSDto>>> GetShrineKamiByIdCMSAsync([FromRoute] int id)
    {
        var result = await _mediator.Send(new GetShrineKamiByIdCMSQuery(id));
        return Ok(result.Kami);
    }

    // GET /api/shrines/cms/kami
    [HttpGet("cms/kami")]
    [Authorize]
    public async Task<ActionResult<IReadOnlyList<KamiReadCMSDto>>> GetAllKamiListCMSAsync()
    {
        var result = await _mediator.Send(new GetAllKamiListCMSQuery());
        return Ok(result.Kami);
    }

    // GET /api/shrines/cms/{id}/history
    [HttpGet("cms/{id}/history")]
    [Authorize]
    public async Task<ActionResult<IReadOnlyList<HistoryReadCMSDto>>> GetShrineHistoryByIdCMSAsync([FromRoute] int id)
    {
        var result = await _mediator.Send(new GetShrineHistoryByIdCMSQuery(id));
        return Ok(result.History);
    }

    // GET /api/shrines/cms/{id}/folklore
    [HttpGet("cms/{id}/folklore")]
    [Authorize]
    public async Task<ActionResult<IReadOnlyList<FolkloreReadCMSDto>>> GetShrineFolkloreByIdCMSAsync([FromRoute] int id)
    {
        var result = await _mediator.Send(new GetShrineFolkloreByIdCMSQuery(id));
        return Ok(result.Folklore);
    }

    // GET /api/shrines/cms/{id}/gallery
    [HttpGet("cms/{id}/gallery")]
    public async Task<ActionResult<IReadOnlyList<ImageCMSAuditDto>>> GetShrineGalleryByIdCMSAsync([FromRoute] int id)
    {
        var result = await _mediator.Send(new GetShrineGalleryByIdCMSQuery(id));
        return Ok(result.Images);
    }

    // Returns all tags
    // GET /api/shrines/cms/tags
    [HttpGet("cms/tags")]
    [Authorize]
    public async Task<ActionResult<IReadOnlyList<TagDto>>> GetAllTagsCMSAsync()
    {
        var result = await _mediator.Send(new GetAllTagsListCMSQuery());
        return Ok(result.Tags);
    }

    // Returns audit of shrine
    // GET /api/shrines/cms/{id}/audit
    [Authorize]
    [HttpGet("cms/{id}/audit")]
    public async Task<ActionResult<ShrineAuditDto>> GetShrineAudit([FromRoute] int id)
    {
        if (id <= 0)
            return BadRequest("Invalid shrine id.");
        
        var result = await _mediator.Send(new GetShrineAuditQuery(id));
        return Ok(result);
    }

    // Returns citations linked to shrine
    // GET /api/shrines/cms/{id}/citations
    [Authorize]
    [HttpGet("cms/{id}/citations")]
    public async Task<ActionResult<IReadOnlyList<ShrineCitationCMSDto>>> GetShrineCitationsByIdCMSAsync([FromRoute] int id)
    {
        if (id <= 0)
            return BadRequest("Invalid shrine id.");
        
        var result = await _mediator.Send(new GetShrineCitationsByIdCMSQuery(id));
        return Ok(result.Citations);
    }

    // Returns citations linked to shrine
    // GET /api/shrines/cms/{id}/citations-dropdown
    [Authorize]
    [HttpGet("cms/{id}/citations-dropdown")]
    public async Task<ActionResult<IReadOnlyList<CitationCMSDto>>> GetShrineCitationsDropdownByIdCMSAsync([FromRoute] int id)
    {
        if (id <= 0)
            return BadRequest("Invalid shrine id.");
        
        var result = await _mediator.Send(new GetShrineCitationsDropdownByIdCMSQuery(id));
        return Ok(result.Citations);
    }

    // Returns shrine status
    [Authorize]
    [HttpGet("cms/{id}/status")]
    public async Task<ActionResult<string>> GetShrineStatusByIdCMSAsync([FromRoute] int id)
    {
        if (id <= 0)
            return BadRequest("Invalid shrine id.");

        var result = await _mediator.Send(new GetShrineStatusByIdCMSQuery(id));
        return Ok(result.Status);
    }

    // Returns import preview
    // POST /api/shrines/cms/import-preview
    [Authorize]
    [HttpPost("cms/import-preview")]
    public async Task<ActionResult<IReadOnlyList<ImportPreviewItemDto>>> GetImportPreviewCMSAsync([FromBody] ImportPreviewRequest request)
    {
        // validate request items
        if (request is null) return BadRequest("Request body is required."); 
        if (string.IsNullOrWhiteSpace(request.Location)) return BadRequest("Location is required.");
        if (!Enum.IsDefined(typeof(SearchSize), request.SearchSize)) return BadRequest("Invalid search size.");
        if (request.MaxResults <= 0 || request.MaxResults > 100) return BadRequest("MaxResults must be between 1 and 100.");

        var result = await _mediator.Send(new GetImportPreviewCMSQuery(request));
        return Ok(result.Preview);
    }
}

public record ImportPreviewRequest(
    string Location,
    SearchSize SearchSize,
    int MaxResults
);