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
        if (string.IsNullOrWhiteSpace(input.Location))
            throw new ValidationException("Location is required.");

        if (input.MaxResults <= 0 || input.MaxResults > 100)
            throw new ValidationException("MaxResults must be between 1 and 100.");

        // 2. GET LAT / LON (LocationIQ)
        var (lat, lon) = await _geoService.GeocodeAsync(input.Location, ct);

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
  node(around:{radius},{lat},{lon})[""amenity""=""place_of_worship""][""religion""=""shinto""];
  way(around:{radius},{lat},{lon})[""amenity""=""place_of_worship""][""religion""=""shinto""];
  relation(around:{radius},{lat},{lon})[""amenity""=""place_of_worship""][""religion""=""shinto""];
);
out center {input.MaxResults};
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
            .Take(input.MaxResults)
            .ToList();

        // 9. RETURN
        return new GetImportPreviewCMSResult(final);
    }
}