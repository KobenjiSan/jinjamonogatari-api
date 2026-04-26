using Application.Features.Shrines.Services;
using MediatR;

namespace Application.Features.Shrines.Queries.GetShrineCounts;

public class GetShrineCountsHandler : IRequestHandler<GetShrineCountsQuery, GetShrineCountsResult>
{
    private readonly IShrineReadService _readService;

    public GetShrineCountsHandler(IShrineReadService readService)
    {
        _readService = readService;
    }

    public async Task<GetShrineCountsResult> Handle(GetShrineCountsQuery request, CancellationToken ct)
    {
        return await _readService.GetShrineCountsAsync(ct);
    }

}