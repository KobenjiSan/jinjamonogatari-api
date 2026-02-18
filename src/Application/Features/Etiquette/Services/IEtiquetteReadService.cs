using Application.Features.Etiquette.Models;
using Application.Features.Etiquette.Queries.GetEtiquetteTopicDetail;

namespace Application.Features.Etiquette.Services;

public interface IEtiquetteReadService
{
    Task<IReadOnlyList<EtiquetteTopicSummaryDto>> GetTopicsAsync(CancellationToken ct);

    Task<GetEtiquetteTopicDetailResult?> GetTopicDetailByIdAsync(int topicId, CancellationToken ct);
    Task<GetEtiquetteTopicDetailResult?> GetTopicDetailBySlugAsync(string slug, CancellationToken ct);
}
