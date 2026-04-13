using Application.Features.Shrines.Models;
using Application.Features.Shrines.Services;
using Application.Features.Shrines.Services.ShrineAudit;
using MediatR;

namespace Application.Features.Shrines.Queries.GetShrineListCMS;

public class GetShrineListCMSHandler : IRequestHandler<GetShrineListCMSQuery, GetShrineListCMSResult>
{
    private readonly IShrineReadService _readService;
    private readonly IShrineAuditService _shrineAuditService;

    public GetShrineListCMSHandler(IShrineReadService readService, IShrineAuditService shrineAuditService)
    {
        _readService = readService;
        _shrineAuditService = shrineAuditService;
    }

    public async Task<GetShrineListCMSResult> Handle(GetShrineListCMSQuery request, CancellationToken ct)
    {
        var (shrines, total) = await _readService.GetShrineListCMSAsync(request, ct);

        var updatedShrines = new List<ShrineListCMSDto>();

        foreach (var shrine in shrines)
        {
            var errorCount = 0;

            var snapshot = await _readService.GetShrineAuditSnapshotAsync(shrine.ShrineId, ct);
            if (snapshot is not null)
            {
                var audit = _shrineAuditService.Evaluate(snapshot);
                errorCount = audit.ErrorCount;
            }

            var recentlyRejected = await _readService.IsShrineRecentlyRejectedAsync(shrine.ShrineId, ct);

            updatedShrines.Add(shrine with
            {
                ErrorCount = errorCount,
                RecentlyRejected = recentlyRejected
            });
        }

        return new GetShrineListCMSResult(updatedShrines, total);
    }
}