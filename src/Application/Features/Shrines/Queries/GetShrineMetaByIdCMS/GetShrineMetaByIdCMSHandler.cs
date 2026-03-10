using Application.Common.Exceptions;
using Application.Features.Shrines.Services;
using MediatR;

namespace Application.Features.Shrines.Queries.GetShrineMetaByIdCMS;

public class GetShrineMetaByIdCMSHandler : IRequestHandler<GetShrineMetaByIdCMSQuery, GetShrineMetaByIdCMSResult>
{
    private readonly IShrineReadService _readService;

    public GetShrineMetaByIdCMSHandler(IShrineReadService readService)
    {
        _readService = readService;
    }

    public async Task<GetShrineMetaByIdCMSResult> Handle(GetShrineMetaByIdCMSQuery request, CancellationToken ct)
    {
        var shrineMeta = await _readService.GetShrineMetaByIdCMSAsync(request.Id, ct);
        if (shrineMeta is null) throw new NotFoundException("Shrine not found.");
        return new GetShrineMetaByIdCMSResult(shrineMeta);
    }
}