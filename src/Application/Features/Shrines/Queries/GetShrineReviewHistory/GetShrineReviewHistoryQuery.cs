using Application.Features.Shrines.Models;
using MediatR;

namespace Application.Features.Shrines.Queries.GetShrineReviewHistory;

// QUERIES
public record GetShrineReviewHistoryQuery(int ShrineId) : IRequest<GetShrineReviewHistoryResult>;

// RESULTS
public record GetShrineReviewHistoryResult(IReadOnlyList<ShrineReviewDto> ReviewHistory);