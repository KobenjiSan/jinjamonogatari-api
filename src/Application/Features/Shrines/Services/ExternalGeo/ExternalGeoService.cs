using System.Net.Http;
using System.Text.Json;
using Application.Common.Exceptions;
using Application.Features.Shrines.Models;
using Application.Features.Shrines.Services.ExternalGeo;

public class ExternalGeoService : IExternalGeoService
{
    private readonly HttpClient _http;
    private readonly string _locationIqKey;

    public ExternalGeoService(HttpClient http, IConfiguration config)
    {
        _http = http;
        _locationIqKey = config["LocationIQ:Key"] ?? throw new Exception("LocationIQ key not configured.");
    }

    public async Task<(double Lat, double Lon)> GeocodeAsync(string location, CancellationToken ct)
    {
        var url = $"https://us1.locationiq.com/v1/search?key={_locationIqKey}&q={Uri.EscapeDataString(location)}&format=json&limit=1";

        try
        {
            using var response = await _http.GetAsync(url, ct);

            if (response.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
                throw new BadRequestException("Location lookup is temporarily busy. Please try again in a moment.");

            if (!response.IsSuccessStatusCode)
                throw new BadRequestException("Unable to look up that location right now.");

            var results = await response.Content.ReadFromJsonAsync<List<LocationIqResult>>(cancellationToken: ct);

            if (results == null || results.Count == 0)
                throw new BadRequestException("Location not found. Try a more specific address or place name.");

            var first = results[0];

            return (double.Parse(first.lat), double.Parse(first.lon));
        }
        catch (TaskCanceledException) when (!ct.IsCancellationRequested)
        {
            throw new BadRequestException("Location lookup timed out. Please try again.");
        }
        catch (HttpRequestException)
        {
            throw new BadRequestException("Unable to contact the location service right now.");
        }

    }

    public async Task<List<OverpassElement>> QueryOverpassAsync(string query, CancellationToken ct)
    {
        var url = "https://overpass-api.de/api/interpreter";

        using var content = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("data", query)
        });

        try
        {
            using var response = await _http.PostAsync(url, content, ct);

            if (response.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
                throw new BadRequestException("Map data service is receiving too many requests right now. Please try again shortly.");

            if (!response.IsSuccessStatusCode)
                throw new BadRequestException("Unable to search map data right now.");

            var overpassResponse = await response.Content.ReadFromJsonAsync<OverpassResponse>(cancellationToken: ct);

            if (overpassResponse?.Elements == null || overpassResponse.Elements.Count == 0)
                return new List<OverpassElement>();

            return overpassResponse.Elements.Select(el => new OverpassElement
            {
                Type = el.Type,
                Id = el.Id,
                Lat = el.Lat ?? el.Center?.Lat,
                Lon = el.Lon ?? el.Center?.Lon,
                Tags = el.Tags
            }).ToList();
        }
        catch (TaskCanceledException) when (!ct.IsCancellationRequested)
        {
            throw new BadRequestException("Map search timed out. Please try again.");
        }
        catch (HttpRequestException)
        {
            throw new BadRequestException("Unable to contact the map data service right now.");
        }
    }
}