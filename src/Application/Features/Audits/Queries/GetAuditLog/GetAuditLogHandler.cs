using Application.Features.Audits.Services;
using MediatR;

namespace Application.Features.Audits.Queries.GetAuditLog;

public class GetAuditLogHandler : IRequestHandler<GetAuditLogQuery, GetAuditLogResult>
{
    private readonly IAuditService _service;

    public GetAuditLogHandler(IAuditService service)
    {
        _service = service;
    }

    public async Task<GetAuditLogResult> Handle(GetAuditLogQuery request, CancellationToken ct)
    {
        var (tags, total) = await _service.GetAuditLogAsync(request, ct);
        return new GetAuditLogResult(tags, total);
    }
}