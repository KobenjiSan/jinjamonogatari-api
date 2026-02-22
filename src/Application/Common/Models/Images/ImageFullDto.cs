using Application.Common.Models.Citations;

namespace Application.Common.Models.Images;

// Used for viewing full images with metadata
public record ImageFullDto(
    int ImgId,
    string? ImageUrl,
    string? Title,
    string? Desc,
    CitationDto? Citation
);
