using Application.Features.Shrines.Services;
using MediatR;

namespace Application.Features.Shrines.Queries.GetShrineKamiBySlug;

public class GetShrineKamiBySlugHandler : IRequestHandler<GetShrineKamiBySlugQuery, GetShrineKamiBySlugResult>
{
    private readonly IShrineReadService _readService;

    public GetShrineKamiBySlugHandler(IShrineReadService readService)
    {
        _readService = readService;
    }

    public async Task<GetShrineKamiBySlugResult> Handle(GetShrineKamiBySlugQuery request, CancellationToken ct)
    {
        var kami = await _readService.GetShrineKamiBySlugAsync(request.Slug, ct);
        return new GetShrineKamiBySlugResult(kami);
    }
}