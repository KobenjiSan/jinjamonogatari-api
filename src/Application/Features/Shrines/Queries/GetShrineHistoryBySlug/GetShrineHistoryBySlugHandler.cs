using Application.Features.Shrines.Services;
using MediatR;

namespace Application.Features.Shrines.Queries.GetShrineHistoryBySlug;

public class GetShrineHistoryBySlugHandler : IRequestHandler<GetShrineHistoryBySlugQuery, GetShrineHistoryBySlugResult>
{
    private readonly IShrineReadService _readService;

    public GetShrineHistoryBySlugHandler(IShrineReadService readService)
    {
        _readService = readService;
    }

    public async Task<GetShrineHistoryBySlugResult> Handle(GetShrineHistoryBySlugQuery request, CancellationToken ct)
    {
        var history = await _readService.GetShrineHistoryBySlugAsync(request.Slug, ct);
        return new GetShrineHistoryBySlugResult(history);
    }
}