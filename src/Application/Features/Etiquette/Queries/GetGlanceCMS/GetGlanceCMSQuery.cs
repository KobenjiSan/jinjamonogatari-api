using Application.Features.Etiquette.Models;
using MediatR;

namespace Application.Features.Etiquette.Queries.GetGlanceCMS;

// QUERIES
public record GetGlanceCMSQuery : IRequest<GetGlanceCMSResult>;

// RESULTS
public record GetGlanceCMSResult(IReadOnlyList<AtAGlanceCMSDto> Topics);
