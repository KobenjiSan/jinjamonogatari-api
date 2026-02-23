using Application.Features.Etiquette.Models;
using Application.Features.Etiquette.Queries.GetEtiquetteTopicDetail;

namespace Application.Features.Etiquette.Services;

public interface IEtiquetteReadService
{
    Task<IReadOnlyList<EtiquetteTopicDto>> GetTopicsAsync(CancellationToken ct);

    Task<EtiquetteTopicDto?> GetTopicDetailByIdAsync(int topicId, CancellationToken ct);
    Task<EtiquetteTopicDto?> GetTopicDetailBySlugAsync(string slug, CancellationToken ct);
}
