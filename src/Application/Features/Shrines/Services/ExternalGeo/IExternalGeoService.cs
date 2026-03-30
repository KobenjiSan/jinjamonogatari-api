using Application.Features.Shrines.Models;

namespace Application.Features.Shrines.Services.ExternalGeo;

public interface IExternalGeoService
{
    Task<(double Lat, double Lon)> GeocodeAsync(string location, CancellationToken ct);

    Task<List<OverpassElement>> QueryOverpassAsync(string query, CancellationToken ct);
}