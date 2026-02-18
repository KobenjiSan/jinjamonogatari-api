namespace Application.Features.Etiquette.Models;

public record EtiquetteStepSummaryDto(
    int StepId,
    int? StepOrder,
    string? Text,
    int? ImageId
);
