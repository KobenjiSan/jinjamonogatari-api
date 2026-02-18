using Application.Common.Models.Images;

namespace Application.Features.Etiquette.Models;

public record EtiquetteStepDto(
    int StepId,
    int? StepOrder,
    string? Text,
    ImageDto? Image
);
