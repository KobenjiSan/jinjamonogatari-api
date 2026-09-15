namespace Application.Features.Shrines.Models;

// Used for mapping shrines points on map
public record ShrineMapPointDto(
    int ShrineId,
    string Slug,
    decimal Lat,
    decimal Lon
);

// Used for mapping shrines points on map CMS version
public record ShrineMapPointCMSDto(
    int ShrineId,
    decimal Lat,
    decimal Lon,
    string Status
);

