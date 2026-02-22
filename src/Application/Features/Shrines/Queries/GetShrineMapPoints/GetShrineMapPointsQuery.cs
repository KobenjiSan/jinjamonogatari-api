using Application.Features.Shrines.Models;
using MediatR;

namespace Application.Features.Shrines.Queries.GetShrineMapPoints;

// QUERIES
public record GetShrineMapPointsQuery : IRequest<GetShrineMapPointsResult>;

// RESULTS
public record GetShrineMapPointsResult(IReadOnlyList<ShrineMapPointDto> MapPoints);