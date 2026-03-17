using Application.Features.Shrines.Models;
using MediatR;

namespace Application.Features.Shrines.Queries.GetShrineHistoryByIdCMS;

// QUERIES
public record GetShrineHistoryByIdCMSQuery(int Id) : IRequest<GetShrineHistoryByIdCMSResult>;

// RESULTS
public record GetShrineHistoryByIdCMSResult(IReadOnlyList<HistoryReadCMSDto> History);