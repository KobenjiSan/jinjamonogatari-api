namespace Application.Features.Shrines.Models;

public record AddressDto(
    string? AddressRaw,
    string? Prefecture,
    string? City,
    string? Ward,
    string? Locality,
    string? PostalCode,
    string? Country
);

/*
Example address:

Tokyo Tower (東京タワー)
〒105-0011
東京都港区芝公園4-2-8
Japan
or
4-2-8 Shibakoen, Minato-ku, Tokyo 105-0011, Japan

Meaning:
AddressRaw = "4-2-8 Shibakoen, Minato-ku, Tokyo 105-0011, Japan",
Prefecture = "Tokyo",
City = "Minato",
Ward = null,
Locality = "Shibakoen 4-2-8",
PostalCode = "105-0011",
Country = "Japan"

(note: Minato is already a special ward (no sub-ward))
*/