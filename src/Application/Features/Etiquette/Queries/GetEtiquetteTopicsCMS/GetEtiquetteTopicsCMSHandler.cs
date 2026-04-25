using Application.Features.Etiquette.Services;
using MediatR;

namespace Application.Features.Etiquette.Queries.GetEtiquetteTopicsCMS;

public class GetEtiquetteTopicsCMSHandler : IRequestHandler<GetEtiquetteTopicsCMSQuery, GetEtiquetteTopicsCMSResult>
{
    private readonly IEtiquetteReadService _readService;

    public GetEtiquetteTopicsCMSHandler(IEtiquetteReadService readService)
    {
        _readService = readService;
    }

    public async Task<GetEtiquetteTopicsCMSResult> Handle(GetEtiquetteTopicsCMSQuery request, CancellationToken ct)
    {
        var topics = await _readService.GetTopicsCMSAsync(ct);
        return new GetEtiquetteTopicsCMSResult(topics);
    }
}
