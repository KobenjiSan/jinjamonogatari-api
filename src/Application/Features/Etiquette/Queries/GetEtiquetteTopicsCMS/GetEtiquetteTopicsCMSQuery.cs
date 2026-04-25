using Application.Features.Etiquette.Models;
using MediatR;

namespace Application.Features.Etiquette.Queries.GetEtiquetteTopicsCMS;

// QUERIES
public record GetEtiquetteTopicsCMSQuery : IRequest<GetEtiquetteTopicsCMSResult>;

// RESULTS
public record GetEtiquetteTopicsCMSResult(IReadOnlyList<EtiquetteTopicCMSDto> Topics);
