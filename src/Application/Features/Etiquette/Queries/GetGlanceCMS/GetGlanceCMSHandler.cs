using Application.Features.Etiquette.Services;
using MediatR;

namespace Application.Features.Etiquette.Queries.GetGlanceCMS;

public class GetGlanceCMSHandler : IRequestHandler<GetGlanceCMSQuery, GetGlanceCMSResult>
{
    private readonly IEtiquetteReadService _readService;

    public GetGlanceCMSHandler(IEtiquetteReadService readService)
    {
        _readService = readService;
    }

    public async Task<GetGlanceCMSResult> Handle(GetGlanceCMSQuery request, CancellationToken ct)
    {
        var topics = await _readService.GetGlanceTopicsCMSAsync(ct);
        return new GetGlanceCMSResult(topics);
    }
}
