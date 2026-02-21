using NetTopologySuite.Geometries;
using Domain.Common;

namespace Domain.Entities;

public class Shrine : IHasTimestamps
{
    public int ShrineId { get; set; }           // Our id
    public string? InputtedId { get; set; }     // OverpassAPI id (for reference)

    // Location
    public decimal? Lat { get; set; }
    public decimal? Lon { get; set; }
    public Point? Location { get; set; }    // PostGIS

    // Identity
    public string? Slug { get; set; }
    public string? NameEn { get; set; }
    public string? NameJp { get; set; }
    public string? ShrineDesc { get; set; }

    // Address
    public string? AddressRaw { get; set; }
    public string? Prefecture { get; set; }
    public string? City { get; set; }
    public string? Ward { get; set; }
    public string? Locality { get; set; }
    public string? PostalCode { get; set; } 
    public string? Country { get; set; }

    // Contact
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public string? Website { get; set; }

    // Hero Image
    public int? ImgId { get; set; }
    public Image? Image { get; set; }   // Navigation property (reference navigation)

    // Publishing
    public string? Status { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? PublishedAt { get; set; }

    // Relationships (collections)
    public List<ShrineTag> ShrineTags { get; set; } = new();
}