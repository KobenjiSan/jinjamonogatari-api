using Application.Features.Etiquette.Models;

namespace Application.Features.Etiquette.Services;

public interface IEtiquetteReadService
{
    Task<IReadOnlyList<EtiquetteTopicDto>> GetTopicsAsync(CancellationToken ct);

    Task<EtiquetteTopicDto?> GetTopicDetailByIdAsync(int topicId, CancellationToken ct);
    Task<EtiquetteTopicDto?> GetTopicDetailBySlugAsync(string slug, CancellationToken ct);

    Task<IReadOnlyList<AtAGlanceCMSDto>> GetGlanceTopicsCMSAsync(CancellationToken ct);
    Task<IReadOnlyList<EtiquetteTopicCMSDto>> GetTopicsCMSAsync(CancellationToken ct);

    Task<IReadOnlyList<EtiquetteStepCMSDto>> GetStepsByIdCMSAsync(int topicId, CancellationToken ct);
    Task<string?> GetStepImagePublicIdCMSAsync(int stepId, CancellationToken ct);
    
}
