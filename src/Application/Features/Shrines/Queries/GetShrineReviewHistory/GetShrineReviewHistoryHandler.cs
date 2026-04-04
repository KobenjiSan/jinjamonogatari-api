using Application.Features.Shrines.Services;
using MediatR;

namespace Application.Features.Shrines.Queries.GetShrineReviewHistory;

public class GetShrineReviewHistoryHandler : IRequestHandler<GetShrineReviewHistoryQuery, GetShrineReviewHistoryResult>
{
    private readonly IShrineReadService _readService;

    public GetShrineReviewHistoryHandler(IShrineReadService readService)
    {
        _readService = readService;
    }

    public async Task<GetShrineReviewHistoryResult> Handle(GetShrineReviewHistoryQuery request, CancellationToken ct)
    {
        var reviewHistory = await _readService.GetShrineReviewHistoryAsync(request.ShrineId, ct);
        return new GetShrineReviewHistoryResult(reviewHistory);
    }
}