namespace Application.Features.Shrines.Models;

public record ImportPreviewItemDto(
    string ImportId,
    string? Name,
    double Lat,
    double Lon,
    string SourceType,
    long OsmId
);

