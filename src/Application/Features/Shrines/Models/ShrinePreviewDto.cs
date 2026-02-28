namespace Application.Features.Shrines.Models;

public record ShrinePreviewDto(
    int ShrineId,
    string Slug,

    double? Lat,
    double? Lon,

    string? NameEn,
    string? NameJp,
    string? ImageUrl,
    string? ShrineDesc,
    double? DistanceMeters,
    IReadOnlyList<TagDto> Tags
);

