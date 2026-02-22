using Application.Features.Shrines.Services;
using MediatR;

namespace Application.Features.Shrines.Queries.GetShrineMapPoints;

public class GetShrineMapPointsHandler : IRequestHandler<GetShrineMapPointsQuery, GetShrineMapPointsResult>
{
    private readonly IShrineReadService _readService;

    public GetShrineMapPointsHandler(IShrineReadService readService)
    {
        _readService = readService;
    }

    public async Task<GetShrineMapPointsResult> Handle(GetShrineMapPointsQuery request, CancellationToken ct)
    {
        var points = await _readService.GetShrineMapPointsAsync(ct);
        return new GetShrineMapPointsResult(points);
    }
}