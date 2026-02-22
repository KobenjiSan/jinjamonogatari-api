using Application.Common.Models.Citations;
using Application.Common.Models.Images;

namespace Application.Features.Shrines.Models;

public record HistoryReadDto(
    int HistoryId,
    DateOnly EventDate,
    int SortOrder,
    string Title,
    string? Information,
    ImageCitedDto? Image,
    IReadOnlyList<CitationDto> Citations
);

