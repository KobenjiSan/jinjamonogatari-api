using Application.Features.Shrines.Models;
using MediatR;

namespace Application.Features.Shrines.Queries.GetShrineListView;

// QUERIES
public record GetShrineListViewQuery : IRequest<GetShrineListViewResult>;

// RESULTS
public record GetShrineListViewResult(IReadOnlyList<ShrineCardDto> Shrines);