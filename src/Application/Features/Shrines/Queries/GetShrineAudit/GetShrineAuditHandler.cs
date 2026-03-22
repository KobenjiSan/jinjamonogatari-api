using Application.Features.Shrines.Models;
using Application.Features.Shrines.Services;
using Application.Features.Shrines.Services.ShrineAudit;
using MediatR;

namespace Application.Features.Shrines.Queries.GetShrineAudit;

public class GetShrineAuditHandler : IRequestHandler<GetShrineAuditQuery, ShrineAuditDto>
{
    private readonly IShrineReadService _shrineReadService;
    private readonly IShrineAuditService _shrineAuditService;

    public GetShrineAuditHandler(
        IShrineReadService shrineReadService,
        IShrineAuditService shrineAuditService)
    {
        _shrineReadService = shrineReadService;
        _shrineAuditService = shrineAuditService;
    }

    public async Task<ShrineAuditDto> Handle(GetShrineAuditQuery request, CancellationToken ct)
    {
        var snapshot = await _shrineReadService.GetShrineAuditSnapshotAsync(request.ShrineId, ct);

        if (snapshot is null)
            throw new KeyNotFoundException("Shrine not found.");

        return _shrineAuditService.Evaluate(snapshot);
    }
}