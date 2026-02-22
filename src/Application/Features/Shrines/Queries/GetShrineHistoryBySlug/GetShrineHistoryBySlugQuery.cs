using Application.Features.Shrines.Models;
using MediatR;

namespace Application.Features.Shrines.Queries.GetShrineHistoryBySlug;

// QUERIES
public record GetShrineHistoryBySlugQuery(string Slug) : IRequest<GetShrineHistoryBySlugResult>;

// RESULTS
public record GetShrineHistoryBySlugResult(IReadOnlyList<HistoryReadDto> History);