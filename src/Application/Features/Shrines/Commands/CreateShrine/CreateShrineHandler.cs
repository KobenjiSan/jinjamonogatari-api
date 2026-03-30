using System.ComponentModel.DataAnnotations;
using Application.Features.Shrines.Services;
using Application.Features.Shrines.Services.ExternalGeo;
using MediatR;

namespace Application.Features.Shrines.Commands.CreateShrine;

public class CreateShrineHandler : IRequestHandler<CreateShrineCommand, Unit>
{
    private readonly IShrineWriteService _shrineWriteService;
    private readonly IExternalGeoService _geoService;

    public CreateShrineHandler(
        IShrineWriteService shrineWriteService,
        IExternalGeoService geoService
    )
    {
        _shrineWriteService = shrineWriteService;
        _geoService = geoService;
    }

    public async Task<Unit> Handle(CreateShrineCommand request, CancellationToken ct)
    {
        var req = request.Request;

        var hasAddress = !string.IsNullOrWhiteSpace(req.Address);
        var hasBothCoords = req.Lat is not null && req.Lon is not null;
        var hasOnlyOneCoord = (req.Lat is null) != (req.Lon is null);

        if (string.IsNullOrWhiteSpace(req.NameEn) && string.IsNullOrWhiteSpace(req.NameJp))
            throw new ValidationException("Each shrine must have a name to be created.");

        if (hasOnlyOneCoord)
            throw new ValidationException("Latitude and longitude must both be provided together.");

        if (!hasAddress && !hasBothCoords)
            throw new ValidationException("Each shrine must include either an address or both latitude and longitude.");

        if (hasAddress && !hasBothCoords)
        {
            var (lat, lon) = await _geoService.GeocodeAsync(req.Address!, ct);
            req = req with
            {
                Lat = lat,
                Lon = lon
            };
        }

        if (req.Lat is not null && (req.Lat < -90 || req.Lat > 90))
            throw new ValidationException("Latitude must be between -90 and 90.");

        if (req.Lon is not null && (req.Lon < -180 || req.Lon > 180))
            throw new ValidationException("Longitude must be between -180 and 180.");

        await _shrineWriteService.CreateShrineAsync(req, ct);

        return Unit.Value;
    }
}