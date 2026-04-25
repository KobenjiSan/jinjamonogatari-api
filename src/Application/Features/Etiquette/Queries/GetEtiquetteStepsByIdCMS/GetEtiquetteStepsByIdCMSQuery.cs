using Application.Features.Etiquette.Models;
using MediatR;

namespace Application.Features.Etiquette.Queries.GetEtiquetteStepsByIdCMS;

// QUERIES
public record GetEtiquetteStepsByIdCMSQuery(int TopicId) : IRequest<GetEtiquetteStepsByIdCMSResult>;

// RESULTS
public record GetEtiquetteStepsByIdCMSResult(IReadOnlyList<EtiquetteStepCMSDto> Steps);
