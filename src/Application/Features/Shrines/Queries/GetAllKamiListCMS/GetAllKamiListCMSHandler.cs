using Application.Features.Shrines.Services;
using MediatR;

namespace Application.Features.Shrines.Queries.GetAllKamiListCMS;

public class GetAllKamiListCMSHandler : IRequestHandler<GetAllKamiListCMSQuery, GetAllKamiListCMSResult>
{
    private readonly IShrineReadService _readService;

    public GetAllKamiListCMSHandler(IShrineReadService readService)
    {
        _readService = readService;
    }

    public async Task<GetAllKamiListCMSResult> Handle(GetAllKamiListCMSQuery request, CancellationToken ct)
    {
        var kami = await _readService.GetAllKamiListCMSAsync(ct);
        return new GetAllKamiListCMSResult(kami);
    }
}