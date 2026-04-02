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

        foreach(var shrine in shrines)
        {
            var snapshot = await _readService.GetShrineAuditSnapshotAsync(shrine.ShrineId, ct);

            if (snapshot is null)
            {
                updatedShrines.Add(shrine with
                {
                    ErrorCount = 0
                });
                continue;
            }

            var audit = _shrineAuditService.Evaluate(snapshot);

            updatedShrines.Add(shrine with
            {
                ErrorCount = audit.ErrorCount
            });
        }

        shrines = updatedShrines;

        return new GetShrineListCMSResult(shrines, total);
    }
}