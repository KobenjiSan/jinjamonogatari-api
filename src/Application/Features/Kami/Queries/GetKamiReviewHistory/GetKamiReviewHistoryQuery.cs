using Application.Features.Kami.Models;
using MediatR;

namespace Application.Features.Kami.Queries.GetKamiReviewHistory;

// QUERIES
public record GetKamiReviewHistoryQuery(int KamiId) : IRequest<GetKamiReviewHistoryResult>;

// RESULTS
public record GetKamiReviewHistoryResult(IReadOnlyList<KamiReviewDto> ReviewHistory);