using Application.Features.Etiquette.Models;
using Application.Features.Etiquette.Queries.GetEtiquetteTopicDetail;
using Application.Features.Etiquette.Queries.GetEtiquetteTopics;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/etiquette")]
public class EtiquetteReadController : ControllerBase
{
    private readonly IMediator _mediator;

    public EtiquetteReadController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // GET /api/etiquette
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<EtiquetteTopicSummaryDto>>> GetTopicsAsync()
    {
        var result = await _mediator.Send(new GetEtiquetteTopicsQuery());
        return Ok(result.Topics);
    }

    // GET /api/etiquette/3
    [HttpGet("{id:int}")]
    public async Task<ActionResult<EtiquetteTopicDetailDto>> GetTopicDetailByIdAsync(int id)
    {
        var result = await _mediator.Send(new GetEtiquetteTopicDetailByIdQuery(id));
        return Ok(result.Topic);
    }

    // GET /api/etiquette/slug/temizu
    [HttpGet("slug/{slug}")]
    public async Task<ActionResult<EtiquetteTopicDetailDto>> GetTopicDetailBySlugAsync(string slug)
    {
        var result = await _mediator.Send(new GetEtiquetteTopicDetailBySlugQuery(slug));
        return Ok(result.Topic);
    }
}
