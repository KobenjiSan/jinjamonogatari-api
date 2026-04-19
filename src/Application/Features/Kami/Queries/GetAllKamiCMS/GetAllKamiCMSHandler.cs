using Application.Features.Kami.Services;
using Application.Features.Shrines.Services;
using Application.Features.Tags.Services;
using MediatR;

namespace Application.Features.Kami.Queries.GetAllKamiCMS;

public class GetAllKamiCMSHandler : IRequestHandler<GetAllKamiCMSQuery, GetAllKamiCMSResult>
{
    private readonly IKamiService _service;

    public GetAllKamiCMSHandler(IKamiService service)
    {
        _service = service;
    }

    public async Task<GetAllKamiCMSResult> Handle(GetAllKamiCMSQuery request, CancellationToken ct)
    {
        var (kami, total) = await _service.GetAllKamiCMSAsync(request, ct);
        return new GetAllKamiCMSResult(kami, total);
    }
}