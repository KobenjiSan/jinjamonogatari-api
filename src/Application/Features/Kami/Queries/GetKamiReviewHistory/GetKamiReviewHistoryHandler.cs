using Application.Features.Kami.Services;
using Application.Features.Shrines.Services;
using MediatR;

namespace Application.Features.Kami.Queries.GetKamiReviewHistory;

public class GetKamiReviewHistoryHandler : IRequestHandler<GetKamiReviewHistoryQuery, GetKamiReviewHistoryResult>
{
    private readonly IKamiService _service;

    public GetKamiReviewHistoryHandler(IKamiService service)
    {
        _service = service;
    }

    public async Task<GetKamiReviewHistoryResult> Handle(GetKamiReviewHistoryQuery request, CancellationToken ct)
    {
        var reviewHistory = await _service.GetKamiReviewHistoryAsync(request.KamiId, ct);
        return new GetKamiReviewHistoryResult(reviewHistory);
    }
}