using Application.Common.Models.Citations;
using MediatR;

namespace Application.Features.Shrines.Queries.GetShrineCitationsByIdCMS;

// QUERY
public record GetShrineCitationsByIdCMSQuery(int ShrineId)
    : IRequest<GetShrineCitationsByIdCMSResult>;

// RESULT
public record GetShrineCitationsByIdCMSResult(
    IReadOnlyList<ShrineCitationCMSDto> Citations
);