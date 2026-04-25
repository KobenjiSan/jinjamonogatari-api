using Application.Common.Models.Images;

namespace Application.Features.Etiquette.Models;

public record EtiquetteStepCMSDto(
    int StepId,
    string? Text,
    int? StepOrder,
    ImageCMSDto? Image
);
