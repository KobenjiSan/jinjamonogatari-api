using Application.Features.Shrines.Services;
using MediatR;

namespace Application.Features.Shrines.Queries.GetShrineListCMS;

public class GetShrineListCMSHandler : IRequestHandler<GetShrineListCMSQuery, GetShrineListCMSResult>
{
    private readonly IShrineReadService _readService;

    public GetShrineListCMSHandler(IShrineReadService readService)
    {
        _readService = readService;
    }

    public async Task<GetShrineListCMSResult> Handle(GetShrineListCMSQuery request, CancellationToken ct)
    {
        var shrines = await _readService.GetShrineListCMSAsync(ct);
        return new GetShrineListCMSResult(shrines);
    }
}