using Application.Common.Models.Images;

namespace Application.Features.Shrines.Models;

// Used for meta section in Shrine Editor
public record ShrineMetaCMSDto(
    // System
    int ShrineId,
    string InputtedId,

    // Identity
    string? Slug,
    string? NameEn,
    string? NameJp,
    string? ShrineDesc,

    // Location
    decimal? Lat,
    decimal? Lon,

    // Address
    string? Prefecture,
    string? City,
    string? Ward,
    string? Locality,
    string? PostalCode,
    string? Country,

    // Contact
    string? PhoneNumber,
    string? Email,
    string? Website,

    // Hero Image
    ImageFullDto? Image,

    // Publishing
    string Status,
    DateTime? PublishedAt,

    // Timestamps
    DateTime CreatedAt,
    DateTime UpdatedAt,

    // Tags
    IReadOnlyList<TagDto> Tags
);