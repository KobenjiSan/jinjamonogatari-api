using NetTopologySuite.Geometries;
using Domain.Common;

namespace Domain.Entities;

public class Shrine : IHasTimestamps
{
    // PK
    public int ShrineId { get; set; }
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

    // Timestamps
    public DateTime? PublishedAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Notes
    public string? Notes { get; set; }
    

    // Relationships (collections)
    public ICollection<ShrineTag> ShrineTags { get; set; } = new List<ShrineTag>();
    public ICollection<ShrineKami> ShrineKamis { get; set; } = new List<ShrineKami>();
    public ICollection<History> ShrineHistories { get; set; } = new List<History>();
    public ICollection<Folklore> ShrineFolklores { get; set; } = new List<Folklore>();
    public ICollection<ShrineGallery> ShrineGalleries { get; set; } = new List<ShrineGallery>();
    public ICollection<UserCollection> UserCollections { get; set; } = new List<UserCollection>();
    public ICollection<ShrineReview> Reviews { get; set; } = new List<ShrineReview>();
}