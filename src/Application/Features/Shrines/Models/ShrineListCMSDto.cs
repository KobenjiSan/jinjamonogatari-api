namespace Application.Features.Shrines.Models;
public record ShrineListCMSDto(
    // Id
    int ShrineId,
    // Shrine Name
    string? NameEn,
    string? NameJp,
    // Status
    string? Status,
    // Location
    string? City,
    decimal? Lat,
    decimal? Lon,
    // Last Updated
    DateTime UpdatedAt
);