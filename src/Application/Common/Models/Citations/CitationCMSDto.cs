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

public record ShrineCitationCMSDto(
    CitationCMSDto Citation,
    int UsageCount,
    List<CitationLinkedItemDto> LinkedTo
);

public record CitationLinkedItemDto(
    string Type,
    int Id,
    string Name
);