using Application.Common.Models.Citations;
using MediatR;

namespace Application.Features.Shrines.Queries.GetShrineCitationsDropdownByIdCMS;

// QUERY
public record GetShrineCitationsDropdownByIdCMSQuery(int ShrineId)
    : IRequest<GetShrineCitationsDropdownByIdCMSResult>;

// RESULT
public record GetShrineCitationsDropdownByIdCMSResult(IReadOnlyList<CitationCMSDto> Citations);