using Application.Common.Models.Citations;

namespace Application.Common.Models.Images;
public record ImageCMSDto(
    int ImgId,
    string? ImageUrl,
    string? Title,
    string? Desc,
    CitationCMSDto? Citation,
    DateTime CreatedAt,
    DateTime UpdatedAt
);