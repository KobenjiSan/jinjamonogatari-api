using Application.Features.Etiquette.Models;
using MediatR;

namespace Application.Features.Etiquette.Queries.GetEtiquetteTopicDetail;

// QUERIES
public record GetEtiquetteTopicDetailByIdQuery(int TopicId) : IRequest<GetEtiquetteTopicDetailResult>;
public record GetEtiquetteTopicDetailBySlugQuery(string Slug) : IRequest<GetEtiquetteTopicDetailResult>;

// RESULT
public record GetEtiquetteTopicDetailResult(EtiquetteTopicDetailDto Topic);
