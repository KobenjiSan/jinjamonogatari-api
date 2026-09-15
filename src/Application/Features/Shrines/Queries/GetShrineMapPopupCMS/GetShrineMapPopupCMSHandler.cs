using Application.Common.Exceptions;
using Application.Features.Shrines.Services;
using Application.Features.Shrines.Services.ShrineAudit;
using Domain.Entities;
using MediatR;

namespace Application.Features.Shrines.Queries.GetShrineMapPopupCMS;

public class GetShrineMapPopupCMSHandler : IRequestHandler<GetShrineMapPopupCMSQuery, GetShrineMapPopupCMSResult>
{
    private readonly IShrineReadService _readService;
    private readonly IShrineAuditService _shrineAuditService;

    public GetShrineMapPopupCMSHandler(IShrineReadService readService, IShrineAuditService shrineAuditService)
    {
        _readService = readService;
        _shrineAuditService = shrineAuditService;
    }

    public async Task<GetShrineMapPopupCMSResult> Handle(GetShrineMapPopupCMSQuery request, CancellationToken ct)
    {
        var result = await _readService.GetShrineMapPopupCMSAsync(request.ShrineId, ct);

        if (result is null) throw new NotFoundException("Shrine not found.");
        
        var errorCount = 0;

        var snapshot = await _readService.GetShrineAuditSnapshotAsync(result.ShrineId, ct);
        if (snapshot is not null)
        {
            var audit = _shrineAuditService.Evaluate(snapshot);
            errorCount = audit.ErrorCount;
        }

        var recentlyRejected = await _readService.IsShrineRecentlyRejectedAsync(result.ShrineId, ct);
        
        var final = result with
        {
            ErrorCount = errorCount,
            RecentlyRejected = recentlyRejected
        };
        
        return new GetShrineMapPopupCMSResult(final);
    }
}