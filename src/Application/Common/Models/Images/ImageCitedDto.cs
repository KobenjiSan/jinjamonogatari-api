using Application.Common.Models.Citations;

namespace Application.Common.Models.Images;

// Used for basic thumbnail image displays
public record ImageCitedDto(
    string? ImageUrl,
    CitationDto? Citation
);