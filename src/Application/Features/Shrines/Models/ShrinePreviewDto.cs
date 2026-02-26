namespace Application.Features.Shrines.Models;

public record ShrinePreviewDto(
    int ShrineId,
    string Slug,
    string? NameEn,
    string? NameJp,
    string? ImageUrl,
    string? ShrineDesc,
    double? DistanceMeters,
    IReadOnlyList<TagDto> Tags
);

// Add later

// Filtering
// string? Prefecture,
// string? City,

