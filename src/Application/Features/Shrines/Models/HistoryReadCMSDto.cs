using Application.Common.Models.Citations;
using Application.Common.Models.Images;

namespace Application.Features.Shrines.Models;

public record HistoryReadCMSDto(
    int HistoryId,
    DateOnly? EventDate,
    int? SortOrder,
    string? Title,
    string? Information,
    string? Status,
    DateTime? PublishedAt,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    ImageCMSDto? Image,
    IReadOnlyList<CitationCMSDto> Citations
);