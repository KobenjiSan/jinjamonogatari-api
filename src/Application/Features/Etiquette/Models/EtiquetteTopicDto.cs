using Application.Common.Models.Citations;
using Application.Common.Models.Images;

namespace Application.Features.Etiquette.Models;

public record EtiquetteTopicDto(
    int TopicId,
    string Slug,
    string? TitleLong,
    string? TitleShort,
    string? Summary,
    string? IconKey,
    string? IconSet,
    bool ShowInGlance,
    bool ShowAsHighlight,
    int? GlanceOrder,
    int? GuideOrder,
    IReadOnlyList<EtiquetteStepDto> Steps,
    IReadOnlyList<CitationDto> Citations
);
