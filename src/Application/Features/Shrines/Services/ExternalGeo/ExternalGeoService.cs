using System.Net.Http;
using System.Text.Json;
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

        var results = await _http.GetFromJsonAsync<List<LocationIqResult>>(url, ct);

        if (results == null || results.Count == 0) throw new Exception("Location not found.");

        var first = results[0];

        return (double.Parse(first.lat), double.Parse(first.lon));
    }

    public async Task<List<OverpassElement>> QueryOverpassAsync(string query, CancellationToken ct)
    {
        var url = "https://overpass-api.de/api/interpreter";

        using var content = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("data", query)
        });

        var response = await _http.PostAsync(url, content, ct);
        response.EnsureSuccessStatusCode();

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
}