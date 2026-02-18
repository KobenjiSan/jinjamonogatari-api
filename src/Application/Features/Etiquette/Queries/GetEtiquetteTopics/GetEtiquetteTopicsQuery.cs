using Application.Features.Etiquette.Models;
using MediatR;

namespace Application.Features.Etiquette.Queries.GetEtiquetteTopics;

// QUERIES
public record GetEtiquetteTopicsQuery : IRequest<GetEtiquetteTopicsResult>;

// RESULTS
public record GetEtiquetteTopicsResult(IReadOnlyList<EtiquetteTopicSummaryDto> Topics);
