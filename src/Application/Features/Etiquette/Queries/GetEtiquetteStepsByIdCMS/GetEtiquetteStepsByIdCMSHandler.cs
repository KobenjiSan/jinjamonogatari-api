using Application.Features.Etiquette.Services;
using MediatR;

namespace Application.Features.Etiquette.Queries.GetEtiquetteStepsByIdCMS;

public class GetEtiquetteStepsByIdCMSHandler : IRequestHandler<GetEtiquetteStepsByIdCMSQuery, GetEtiquetteStepsByIdCMSResult>
{
    private readonly IEtiquetteReadService _readService;

    public GetEtiquetteStepsByIdCMSHandler(IEtiquetteReadService readService)
    {
        _readService = readService;
    }

    public async Task<GetEtiquetteStepsByIdCMSResult> Handle(GetEtiquetteStepsByIdCMSQuery request, CancellationToken ct)
    {
        var steps = await _readService.GetStepsByIdCMSAsync(request.TopicId, ct);
        return new GetEtiquetteStepsByIdCMSResult(steps);
    }
}
