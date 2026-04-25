using Api.Controllers.Etiquette;

namespace Application.Features.Etiquette.Services;

public interface IEtiquetteWriteService
{
    Task CreateEtiquetteAsync(CreateEtiquetteRequest request, CancellationToken ct);
    Task UpdateEtiquetteAsync(int topicId, UpdateEtiquetteRequest request, CancellationToken ct);
    Task DeleteEtiquetteAsync(int topicId, CancellationToken ct);

    Task CreateStepAsync(int topicId, CreateStepRequest request, string? publicId, CancellationToken ct);
    Task UpdateStepAsync(int stepId, UpdateStepRequest request, string? publicId, CancellationToken ct);
    Task DeleteStepAsync(int stepId, CancellationToken ct);

    Task UpdateGlanceAsync(int topicId, UpdateGlanceRequest request, CancellationToken ct);

    Task DeleteGlanceAsync(int topicId, CancellationToken ct);
}