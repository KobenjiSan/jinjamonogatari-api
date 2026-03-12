using Application.Features.Shrines.Services;
using MediatR;

namespace Application.Features.Shrines.Queries.GetShrineKamiByIdCMS;

public class GetShrineKamiByIdCMSHandler : IRequestHandler<GetShrineKamiByIdCMSQuery, GetShrineKamiByIdCMSResult>
{
    private readonly IShrineReadService _readService;

    public GetShrineKamiByIdCMSHandler(IShrineReadService readService)
    {
        _readService = readService;
    }

    public async Task<GetShrineKamiByIdCMSResult> Handle(GetShrineKamiByIdCMSQuery request, CancellationToken ct)
    {
        var kami = await _readService.GetShrineKamiByIdCMSAsync(request.Id, ct);
        return new GetShrineKamiByIdCMSResult(kami);
    }
}