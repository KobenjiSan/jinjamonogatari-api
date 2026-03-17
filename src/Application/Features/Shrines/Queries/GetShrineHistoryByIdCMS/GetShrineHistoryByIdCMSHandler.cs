using Application.Features.Shrines.Services;
using MediatR;

namespace Application.Features.Shrines.Queries.GetShrineHistoryByIdCMS;

public class GetShrineHistoryByIdCMSHandler : IRequestHandler<GetShrineHistoryByIdCMSQuery, GetShrineHistoryByIdCMSResult>
{
    private readonly IShrineReadService _readService;

    public GetShrineHistoryByIdCMSHandler(IShrineReadService readService)
    {
        _readService = readService;
    }

    public async Task<GetShrineHistoryByIdCMSResult> Handle(GetShrineHistoryByIdCMSQuery request, CancellationToken ct)
    {
        var history = await _readService.GetShrineHistoryByIdCMSAsync(request.Id, ct);
        return new GetShrineHistoryByIdCMSResult(history);
    }
}