using Application.Features.Etiquette.Services;
using MediatR;

namespace Application.Features.Etiquette.Queries.GetEtiquetteTopics;

public class GetEtiquetteTopicsHandler : IRequestHandler<GetEtiquetteTopicsQuery, GetEtiquetteTopicsResult>
{
    private readonly IEtiquetteReadService _readService;

    public GetEtiquetteTopicsHandler(IEtiquetteReadService readService)
    {
        _readService = readService;
    }

    public async Task<GetEtiquetteTopicsResult> Handle(GetEtiquetteTopicsQuery request, CancellationToken ct)
    {
        var topics = await _readService.GetTopicsAsync(ct);
        return new GetEtiquetteTopicsResult(topics);
    }
}
