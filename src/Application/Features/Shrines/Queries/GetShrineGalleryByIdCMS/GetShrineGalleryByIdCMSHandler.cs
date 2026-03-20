using Application.Features.Shrines.Services;
using MediatR;

namespace Application.Features.Shrines.Queries.GetShrineGalleryByIdCMS;

public class GetShrineGalleryByIdCMSHandler : IRequestHandler<GetShrineGalleryByIdCMSQuery, GetShrineGalleryByIdCMSResult>
{
    private readonly IShrineReadService _readService;

    public GetShrineGalleryByIdCMSHandler(IShrineReadService readService)
    {
        _readService = readService;
    }

    public async Task<GetShrineGalleryByIdCMSResult> Handle(GetShrineGalleryByIdCMSQuery request, CancellationToken ct)
    {
        var images = await _readService.GetShrineGalleryByIdCMSAsync(request.Id, ct);
        return new GetShrineGalleryByIdCMSResult(images);
    }
}