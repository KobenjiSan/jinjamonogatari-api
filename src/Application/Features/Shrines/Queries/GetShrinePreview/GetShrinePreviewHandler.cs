using Application.Common.Exceptions;
using Application.Features.Shrines.Services;
using MediatR;

namespace Application.Features.Shrines.Queries.GetShrinePreview;

public class GetShrinePreviewHandler : IRequestHandler<GetShrinePreviewQuery, GetShrinePreviewResult>
{
    private readonly IShrineReadService _readService;

    public GetShrinePreviewHandler(IShrineReadService readService)
    {
        _readService = readService;
    }

    public async Task<GetShrinePreviewResult> Handle(GetShrinePreviewQuery request, CancellationToken ct)
    {
        var preview = await _readService.GetShrinePreviewAsync(request.Slug, request.Lat, request.Lon, ct);
        if (preview is null) throw new NotFoundException("Shrine not found.");
        return new GetShrinePreviewResult(preview);
    }
}