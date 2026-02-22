using Application.Features.Shrines.Services;
using MediatR;

namespace Application.Features.Shrines.Queries.GetShrineGalleryBySlug;

public class GetShrineGalleryBySlugHandler : IRequestHandler<GetShrineGalleryBySlugQuery, GetShrineGalleryBySlugResult>
{
    private readonly IShrineReadService _readService;

    public GetShrineGalleryBySlugHandler(IShrineReadService readService)
    {
        _readService = readService;
    }

    public async Task<GetShrineGalleryBySlugResult> Handle(GetShrineGalleryBySlugQuery request, CancellationToken ct)
    {
        var images = await _readService.GetShrineGalleryBySlugAsync(request.Slug, ct);
        return new GetShrineGalleryBySlugResult(images);
    }
}