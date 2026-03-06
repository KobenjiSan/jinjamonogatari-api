using Application.Features.Shrines.Models;
using MediatR;

namespace Application.Features.Shrines.Queries.GetShrineListCMS;

// QUERIES
public record GetShrineListCMSQuery() : IRequest<GetShrineListCMSResult>;

// RESULTS
public record GetShrineListCMSResult(IReadOnlyList<ShrineListCMSDto> shrines);