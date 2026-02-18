namespace Application.Common.Models.Citations;

public record CitationDto(
    int CiteId,
    string? Title,
    string? Author,
    string? Url,
    int? Year,
    string? Notes
);
