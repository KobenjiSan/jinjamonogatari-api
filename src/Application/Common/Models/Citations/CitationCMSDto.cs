namespace Application.Common.Models.Citations;
public record CitationCMSDto(
    int CiteId,
    string? Title,
    string? Author,
    string? Url,
    int? Year,
    DateTime CreatedAt,
    DateTime UpdatedAt
);