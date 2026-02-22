using Application.Common.Models.Citations;
using Application.Common.Models.Images;

namespace Application.Features.Shrines.Models;

public record FolkloreReadDto(
    int FolkloreId,
    string Title,
    string Story,
    ImageCitedDto? Image,
    IReadOnlyList<CitationDto> Citations
);

