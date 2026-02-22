using Application.Common.Models.Citations;
using Application.Common.Models.Images;

namespace Application.Features.Shrines.Models;

public record KamiReadDto(
    int KamiId,
    string? NameEn,
    string? NameJp,
    string? Desc,
    ImageCitedDto? Image,
    IReadOnlyList<CitationDto> Citations
);

