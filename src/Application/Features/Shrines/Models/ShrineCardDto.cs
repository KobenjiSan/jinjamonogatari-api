namespace Application.Features.Shrines.Models;

public record ShrineCardDto(
    int ShrineId,
    string Slug,
    string? NameEn,
    string? NameJp,
    string? ImageUrl
);

// Add later

// Filtering
// string? Prefecture,
// string? City,

// Distance
// double? DistanceMeters,

// saved
