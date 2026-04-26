using Application.Features.Audits.Queries.GetAuditLog;
using Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/audits")]
[Authorize(Roles = "Admin")]
public class AuditController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuditController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // GET /api/audits?page=...&pageSize=...&searchQuery=...&sort=...
    [HttpGet]
    public async Task<ActionResult<GetAuditLogResult>> GetAuditLogAsync(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 5,
        [FromQuery] string? searchQuery = null,
        [FromQuery] AuditSort? sort = null
    )
    {
        var result = await _mediator.Send(new GetAuditLogQuery(searchQuery, sort, page, pageSize));
        return Ok(result);
    }
}