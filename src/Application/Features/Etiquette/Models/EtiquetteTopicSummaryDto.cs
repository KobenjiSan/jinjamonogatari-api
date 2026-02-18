using Application.Common.Models.Citations;

namespace Application.Features.Etiquette.Models;

public record EtiquetteTopicSummaryDto(
    int TopicId,
    string Slug,
    string? TitleLong,
    string? TitleShort,
    string? Summary,
    string? IconKey,
    string? IconSet,
    int? ImageId,
    bool ShowInGlance,
    bool ShowAsHighlight,
    int? GlanceOrder,
    int? GuideOrder,
    string? Status,
    DateTime? PublishedAt,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    IReadOnlyList<EtiquetteStepSummaryDto> Steps,
    IReadOnlyList<CitationDto> Citations
);
