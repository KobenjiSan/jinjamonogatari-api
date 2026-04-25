using Application.Features.Etiquette.Models;
using Application.Features.Etiquette.Queries.GetEtiquetteStepsByIdCMS;
using Application.Features.Etiquette.Queries.GetEtiquetteTopicDetail;
using Application.Features.Etiquette.Queries.GetEtiquetteTopics;
using Application.Features.Etiquette.Queries.GetEtiquetteTopicsCMS;
using Application.Features.Etiquette.Queries.GetGlanceCMS;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Etiquette;

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
    public async Task<ActionResult<IReadOnlyList<EtiquetteTopicDto>>> GetTopicsAsync()
    {
        var result = await _mediator.Send(new GetEtiquetteTopicsQuery());
        return Ok(result.Topics);
    }

    // GET /api/etiquette/3
    [HttpGet("{id:int}")]
    public async Task<ActionResult<EtiquetteTopicDto>> GetTopicDetailByIdAsync(int id)
    {
        var result = await _mediator.Send(new GetEtiquetteTopicDetailByIdQuery(id));
        return Ok(result.Topic);
    }

    // GET /api/etiquette/slug/temizu
    [HttpGet("slug/{slug}")]
    public async Task<ActionResult<EtiquetteTopicDto>> GetTopicDetailBySlugAsync(string slug)
    {
        var result = await _mediator.Send(new GetEtiquetteTopicDetailBySlugQuery(slug));
        return Ok(result.Topic);
    }

    // CMS

    // GET /api/etiquette/cms/glance
    [HttpGet("cms/glance")]
    [Authorize]
    public async Task<ActionResult<IReadOnlyList<AtAGlanceCMSDto>>> GetGlanceCMSAsync()
    {
        var result = await _mediator.Send(new GetGlanceCMSQuery());
        return Ok(result.Topics);
    }

    // GET /api/etiquette/cms/topics
    [HttpGet("cms/topics")]
    [Authorize]
    public async Task<ActionResult<IReadOnlyList<EtiquetteTopicCMSDto>>> GetTopicsCMSAsync()
    {
        var result = await _mediator.Send(new GetEtiquetteTopicsCMSQuery());
        return Ok(result.Topics);
    }

    // GET /api/etiquette/cms/steps/{topicId}
    [HttpGet("cms/steps/{topicId}")]
    [Authorize]
    public async Task<ActionResult<IReadOnlyList<EtiquetteStepCMSDto>>> GetStepsByTopicIdCMSAsync([FromRoute] int topicId)
    {
        var result = await _mediator.Send(new GetEtiquetteStepsByIdCMSQuery(topicId));
        return Ok(result.Steps);
    }
}
