using System.ComponentModel.DataAnnotations;
using Application.Common.Exceptions;
using Application.Features.Shrines.Models;
using Application.Features.Shrines.Services;
using Application.Features.Shrines.Services.ExternalGeo;
using Domain.Enums;
using MediatR;

namespace Application.Features.Shrines.Queries.GetImportPreviewCMS;

public class GetImportPreviewCMSHandler 
    : IRequestHandler<GetImportPreviewCMSQuery, GetImportPreviewCMSResult>
{
    private readonly IShrineReadService _readService;
    private readonly IExternalGeoService _geoService;

    public GetImportPreviewCMSHandler(
        IShrineReadService readService,
        IExternalGeoService geoService)
    {
        _readService = readService;
        _geoService = geoService;
    }

    public async Task<GetImportPreviewCMSResult> Handle(GetImportPreviewCMSQuery request, CancellationToken ct)
    {
        var input = request.Request;

        // 1. BASIC VALIDATION
        if (input.Center.Lat < -90 || input.Center.Lat > 90)
            throw new ValidationException("Invalid Center Point.");

        if (input.Center.Lon < -180 || input.Center.Lon > 180)
            throw new ValidationException("Invalid Center Point.");

        if (input.MaxResults <= 0 || input.MaxResults > 100)
            throw new ValidationException("MaxResults must be between 1 and 100.");

        // 2. GET LAT / LON (LocationIQ)
        var (lat, lon) = (input.Center.Lat, input.Center.Lon);

        // 3. MAP SEARCH SIZE → RADIUS
        var radius = input.SearchSize switch
        {
            SearchSize.Small => 1000,
            SearchSize.Medium => 3000,
            SearchSize.Large => 5000,
            _ => throw new ValidationException("Invalid search size.")
        };

        // 4. BUILD OVERPASS QUERY
        var query = $@"
[out:json][timeout:25];
(
  node[""religion""=""shinto""](around:{radius},{lat},{lon});
  way[""religion""=""shinto""](around:{radius},{lat},{lon});
  relation[""religion""=""shinto""](around:{radius},{lat},{lon});

  node[""amenity""=""place_of_worship""][""religion""=""shinto""](around:{radius},{lat},{lon});
  way[""amenity""=""place_of_worship""][""religion""=""shinto""](around:{radius},{lat},{lon});
  relation[""amenity""=""place_of_worship""][""religion""=""shinto""](around:{radius},{lat},{lon});

  node[""building""=""shrine""](around:{radius},{lat},{lon});
  way[""building""=""shrine""](around:{radius},{lat},{lon});
  relation[""building""=""shrine""](around:{radius},{lat},{lon});
);
out center tags;
";

        // 5. CALL OVERPASS
        var elements = await _geoService.QueryOverpassAsync(query, ct);

        // 6. NORMALIZE -> DTO
        var candidates = new List<ImportPreviewItemDto>();

        foreach (var el in elements)
        {
            var latVal = el.Lat;
            var lonVal = el.Lon;

            // skip if no coords
            if (latVal == null || lonVal == null)
                continue;

            string? name = null;
            if (el.Tags != null && el.Tags.TryGetValue("name", out var n))
                name = n;

            var importId = $"{el.Type}/{el.Id}";

            candidates.Add(new ImportPreviewItemDto(
                importId,
                name,
                latVal.Value,
                lonVal.Value,
                el.Type,
                el.Id
            ));
        }

        // 7. FILTER 
        var filtered = candidates
            .Where(x =>
                !string.IsNullOrWhiteSpace(x.Name) && 
                x.Lat != 0 && x.Lon != 0
            )
            .DistinctBy(x => x.ImportId)
            .ToList();

        if (filtered.Count == 0)
            return new GetImportPreviewCMSResult(new List<ImportPreviewItemDto>());

        // 8. REMOVE ALREADY IMPORTED
        var importIds = filtered.Select(x => x.ImportId).ToList();

        var existingIds = await _readService.GetExistingImportIdsAsync(importIds, ct);

        var final = filtered
            .Where(x => !existingIds.Contains(x.ImportId))
            .OrderBy(x => GetDistanceInMeters(
                input.Center.Lat,
                input.Center.Lon,
                x.Lat,
                x.Lon
            ))
            .ThenBy(x => x.ImportId)
            .Take(input.MaxResults)
            .ToList();

        // 9. RETURN
        return new GetImportPreviewCMSResult(final);
    }

    // Haversine formula used to sort by distance on final results
    private static double GetDistanceInMeters(
        double centerLat,
        double centerLon,
        double shrineLat,
        double shrineLon)
    {
        const double earthRadius = 6_371_000;

        var lat1 = centerLat * Math.PI / 180;
        var lat2 = shrineLat * Math.PI / 180;
        var latDifference = (shrineLat - centerLat) * Math.PI / 180;
        var lonDifference = (shrineLon - centerLon) * Math.PI / 180;

        var a =
            Math.Sin(latDifference / 2) * Math.Sin(latDifference / 2) +
            Math.Cos(lat1) * Math.Cos(lat2) *
            Math.Sin(lonDifference / 2) * Math.Sin(lonDifference / 2);

        var angularDistance = 2 * Math.Atan2(
            Math.Sqrt(a),
            Math.Sqrt(1 - a)
        );

        return earthRadius * angularDistance;
    }
}