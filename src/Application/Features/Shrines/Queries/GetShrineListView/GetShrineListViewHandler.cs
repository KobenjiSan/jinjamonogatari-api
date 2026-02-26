using Application.Features.Shrines.Services;
using MediatR;

namespace Application.Features.Shrines.Queries.GetShrineListView;

public class GetShrineListViewHandler : IRequestHandler<GetShrineListViewQuery, GetShrineListViewResult>
{
    private readonly IShrineReadService _readService;

    public GetShrineListViewHandler(IShrineReadService readService)
    {
        _readService = readService;
    }

    public async Task<GetShrineListViewResult> Handle(GetShrineListViewQuery request, CancellationToken ct)
    {
        var shrines = await _readService.GetShrineListViewAsync(request.Lat, request.Lon, ct);
        return new GetShrineListViewResult(shrines);
    }
}