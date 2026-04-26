using Application.Features.Shrines.Models;
using MediatR;

namespace Application.Features.Shrines.Queries.GetShrineCounts;

// QUERIES
public record GetShrineCountsQuery() : IRequest<GetShrineCountsResult>;

// RESULTS
public record GetShrineCountsResult(
    int Total,
    int Imports,
    int Drafts,
    int Review,
    int Published,
    int Rejected
);