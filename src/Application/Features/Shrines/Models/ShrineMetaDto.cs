namespace Application.Features.Shrines.Models;

// Used for main hero / metadata in shrine page
public record ShrineMetaDto(
    int ShrineId,
    string Slug,

    string? NameEn,
    string? NameJp,
    string? ShrineDesc,

    AddressDto? Address,

    string? PhoneNumber,
    string? Email,
    string? Website,

    string? ImageUrl,

    IReadOnlyList<TagDto> Tags
);

// Add later

// Distance
// double? DistanceMeters,
