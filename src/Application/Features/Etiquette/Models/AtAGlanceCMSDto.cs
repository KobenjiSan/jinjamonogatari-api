using Application.Common.Models.Images;

namespace Application.Features.Etiquette.Models;

public record AtAGlanceCMSDto(
    int TopicId,
    string? TitleLong,
    string? TitleShort,
    string? IconKey,
    string? IconSet,
    int? GlanceOrder
);
