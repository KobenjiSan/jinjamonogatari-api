using Application.Common.Models.Citations;

namespace Application.Common.Models.Images;

public record ImageDto(
    int ImgId,
    string? ImgSource,
    string? Title,
    string? Desc,
    CitationDto? Citation
);
