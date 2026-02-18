using Application.Common.Models.Citations;
using Application.Common.Models.Images;

namespace Application.Features.Etiquette.Models;

public record EtiquetteTopicDetailDto(
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
    string? Status,
    DateTime? PublishedAt,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    ImageDto? HeroImage,
    IReadOnlyList<EtiquetteStepDto> Steps,
    IReadOnlyList<CitationDto> Citations
);
