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

        var response = await _http.GetAsync(url, ct);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync(ct);

        using var doc = JsonDocument.Parse(content);

        var root = doc.RootElement;

        if (root.GetArrayLength() == 0)
            throw new Exception("Location not found.");

        var first = root[0];

        var lat = double.Parse(first.GetProperty("lat").GetString()!);
        var lon = double.Parse(first.GetProperty("lon").GetString()!);

        return (lat, lon);
    }

    public async Task<List<OverpassElement>> QueryOverpassAsync(string query, CancellationToken ct)
    {
        var url = "https://overpass-api.de/api/interpreter";

        var content = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("data", query)
        });

        var response = await _http.PostAsync(url, content, ct);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync(ct);

        using var doc = JsonDocument.Parse(json);

        var elements = doc.RootElement.GetProperty("elements");

        var result = new List<OverpassElement>();

        foreach (var el in elements.EnumerateArray())
        {
            var type = el.GetProperty("type").GetString()!;
            var id = el.GetProperty("id").GetInt64();

            double? lat = null;
            double? lon = null;

            if (el.TryGetProperty("lat", out var latProp) &&
                el.TryGetProperty("lon", out var lonProp))
            {
                lat = latProp.GetDouble();
                lon = lonProp.GetDouble();
            }
            else if (el.TryGetProperty("center", out var center))
            {
                lat = center.GetProperty("lat").GetDouble();
                lon = center.GetProperty("lon").GetDouble();
            }

            Dictionary<string, string>? tags = null;

            if (el.TryGetProperty("tags", out var tagsProp))
            {
                tags = new Dictionary<string, string>();

                foreach (var tag in tagsProp.EnumerateObject())
                {
                    tags[tag.Name] = tag.Value.GetString() ?? "";
                }
            }

            result.Add(new OverpassElement
            {
                Type = type,
                Id = id,
                Lat = lat,
                Lon = lon,
                Tags = tags
            });
        }

        return result;
    }
}