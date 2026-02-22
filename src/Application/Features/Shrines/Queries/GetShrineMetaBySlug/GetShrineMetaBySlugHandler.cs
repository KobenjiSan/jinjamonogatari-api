using Application.Common.Exceptions;
using Application.Features.Shrines.Services;
using MediatR;

namespace Application.Features.Shrines.Queries.GetShrineMetaBySlug;

public class GetShrineMetaBySlugHandler : IRequestHandler<GetShrineMetaBySlugQuery, GetShrineMetaBySlugResult>
{
    private readonly IShrineReadService _readService;

    public GetShrineMetaBySlugHandler(IShrineReadService readService)
    {
        _readService = readService;
    }

    public async Task<GetShrineMetaBySlugResult> Handle(GetShrineMetaBySlugQuery request, CancellationToken ct)
    {
        var shrineMeta = await _readService.GetShrineMetaBySlugAsync(request.Slug, ct);
        if (shrineMeta is null) throw new NotFoundException("Shrine not found.");
        return new GetShrineMetaBySlugResult(shrineMeta);
    }
}