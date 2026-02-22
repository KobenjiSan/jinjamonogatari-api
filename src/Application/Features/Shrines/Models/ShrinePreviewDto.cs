namespace Application.Features.Shrines.Models;

public record ShrinePreviewDto(
    int ShrineId,
    string Slug,
    string? NameEn,
    string? NameJp,
    string? ImageUrl,
    string? ShrineDesc,
    IReadOnlyList<TagDto> Tags
);

// Add later

// Filtering
// string? Prefecture,
// string? City,

// Distance
// double? DistanceMeters,

// saved

