using Application.Common.Models.Citations;
using Application.Common.Models.Images;

namespace Application.Features.Shrines.Models;

public record KamiReadCMSDto(
    int KamiId,
    string? NameEn,
    string? NameJp,
    string? Desc,
    string? Status,
    DateTime? PublishedAt,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    ImageCMSDto? Image,
    IReadOnlyList<CitationCMSDto> Citations
);