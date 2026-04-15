namespace Application.Features.Tags.Models;

public record TagCMSDto(
    int TagId,
    string TitleEn,
    string? TitleJp,
    DateTime CreatedAt,
    DateTime UpdatedAt
);

