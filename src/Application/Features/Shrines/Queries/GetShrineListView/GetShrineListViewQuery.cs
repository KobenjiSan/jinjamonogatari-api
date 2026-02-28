using Application.Features.Shrines.Models;
using MediatR;

namespace Application.Features.Shrines.Queries.GetShrineListView;

// QUERIES
public record GetShrineListViewQuery(double? Lat, double? Lon, string? Q) : IRequest<GetShrineListViewResult>;

// RESULTS
public record GetShrineListViewResult(IReadOnlyList<ShrineCardDto> Shrines);