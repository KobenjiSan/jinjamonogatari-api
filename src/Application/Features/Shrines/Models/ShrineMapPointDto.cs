namespace Application.Features.Shrines.Models;

// Used for mapping shrines points on map
public record ShrineMapPointDto(
    int ShrineId,
    string Slug,
    decimal Lat,
    decimal Lon
);
