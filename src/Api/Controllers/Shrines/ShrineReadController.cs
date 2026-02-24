using Application.Common.Models.Images;
using Application.Features.Shrines.Models;
using Application.Features.Shrines.Queries.GetImageById;
using Application.Features.Shrines.Queries.GetShrineFolkloreBySlug;
using Application.Features.Shrines.Queries.GetShrineGalleryBySlug;
using Application.Features.Shrines.Queries.GetShrineHistoryBySlug;
using Application.Features.Shrines.Queries.GetShrineKamiBySlug;
using Application.Features.Shrines.Queries.GetShrineListView;
using Application.Features.Shrines.Queries.GetShrineMapPoints;
using Application.Features.Shrines.Queries.GetShrineMetaBySlug;
using Application.Features.Shrines.Queries.GetShrinePreview;
using MediatR;
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
    public async Task<ActionResult<GetShrineMapPointsResult>> GetShrineMapPointsAsync()
    {
        var result = await _mediator.Send(new GetShrineMapPointsQuery());
        return Ok(result);
    }

    // Map Screen - shrine preview (ShrinePreviewDto)
    // GET /api/shrines/map/{slug}
    [HttpGet("map/{slug}")]
    public async Task<ActionResult<GetShrinePreviewResult>> GetShrinePreviewAsync([FromRoute] string slug)
    {
        var result = await _mediator.Send(new GetShrinePreviewQuery(slug));
        return Ok(result);
    }

    // List Screen - (list of ShrineCardDto) [will need PFS later]
    // GET /api/shrines/list-view
    [HttpGet("list-view")]
    public async Task<ActionResult<GetShrineListViewResult>> GetShrineListViewAsync()
    {
        var result = await _mediator.Send(new GetShrineListViewQuery());
        return Ok(result);
    }

    // Shrine Screens
    // Main page / hero - inital load (ShrineMetaDto)
    // GET /api/shrines/{slug}/meta
    [HttpGet("{slug}/meta")]
    public async Task<ActionResult<GetShrineMetaBySlugResult>> GetShrineMetaBySlugAsync([FromRoute] string slug)
    {
        var result = await _mediator.Send(new GetShrineMetaBySlugQuery(slug));
        return Ok(result);
    }

    // Kami page (list of KamiReadDto)
    // GET /api/shrines/{slug}/kami
    [HttpGet("{slug}/kami")]
    public async Task<ActionResult<GetShrineKamiBySlugResult>> GetShrineKamiBySlugAsync([FromRoute] string slug)
    {
        var result = await _mediator.Send(new GetShrineKamiBySlugQuery(slug));
        return Ok(result);
    }

    // History page (list of HistoryReadDto)
    // GET /api/shrines/{slug}/history
    [HttpGet("{slug}/history")]
    public async Task<ActionResult<GetShrineHistoryBySlugResult>> GetShrineHistoryBySlugAsync([FromRoute] string slug)
    {
        var result = await _mediator.Send(new GetShrineHistoryBySlugQuery(slug));
        return Ok(result);
    }

    // folklore page (list of FolkloreReadDto)
    // GET /api/shrines/{slug}/folklore
    [HttpGet("{slug}/folklore")]
    public async Task<ActionResult<GetShrineFolkloreBySlugResult>> GetShrineFolkloreBySlugAsync([FromRoute] string slug)
    {
        var result = await _mediator.Send(new GetShrineFolkloreBySlugQuery(slug));
        return Ok(result);
    }

    // gallery page (GalleryListItemDto)
    // GET /api/shrines/{slug}/gallery
    [HttpGet("{slug}/gallery")]
    public async Task<ActionResult<GetShrineGalleryBySlugResult>> GetShrineGalleryBySlugAsync([FromRoute] string slug)
    {
        var result = await _mediator.Send(new GetShrineGalleryBySlugQuery(slug));
        return Ok(result);
    }

    // TEMP UNTIL I NEED FULL IMAGE CONTROLLER
    // gallery item clicked (ImageFullDto)
    // GET /api/shrines/image/{id} 
    [HttpGet("image/{id}")]
    public async Task<ActionResult<GetImageByIdResult>> GetImageByIdAsync([FromRoute] int id)
    {
        var result = await _mediator.Send(new GetImageByIdQuery(id));
        return Ok(result);
    }


}