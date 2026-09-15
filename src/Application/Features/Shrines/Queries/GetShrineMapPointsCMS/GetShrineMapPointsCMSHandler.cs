using Application.Features.Shrines.Services;
using MediatR;

namespace Application.Features.Shrines.Queries.GetShrineMapPointsCMS;

public class GetShrineMapPointsCMSHandler : IRequestHandler<GetShrineMapPointsCMSQuery, GetShrineMapPointsCMSResult>
{
    private readonly IShrineReadService _readService;

    public GetShrineMapPointsCMSHandler(IShrineReadService readService)
    {
        _readService = readService;
    }

    public async Task<GetShrineMapPointsCMSResult> Handle(GetShrineMapPointsCMSQuery request, CancellationToken ct)
    {
        var points = await _readService.GetShrineMapPointsCMSAsync(ct);
        return new GetShrineMapPointsCMSResult(points);
    }
}