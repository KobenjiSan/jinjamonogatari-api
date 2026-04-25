using Application.Common.Models.Citations;
using Application.Common.Models.Images;

namespace Application.Features.Etiquette.Models;

public record EtiquetteTopicCMSDto(
    int TopicId,
    string Slug,
    string? TitleLong,
    string? Summary,
    bool ShowInGlance,
    bool ShowAsHighlight,
    int? GuideOrder,
    string? Status,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    IReadOnlyList<CitationCMSDto> Citations
);
