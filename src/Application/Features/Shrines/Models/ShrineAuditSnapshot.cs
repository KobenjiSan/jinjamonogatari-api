namespace Application.Features.Shrines.Models;

public class ShrineAuditSnapshot
{
    public int ShrineId { get; set; }
    public string? InputtedId { get; set; }
    public string? Slug { get; set; }
    public string? NameEn { get; set; }
    public string? NameJp { get; set; }
    public string? ShrineDesc { get; set; }

    public decimal? Lat { get; set; }
    public decimal? Lon { get; set; }

    public string? Prefecture { get; set; }
    public string? City { get; set; }
    public string? Ward { get; set; }
    public string? Locality { get; set; }
    public string? PostalCode { get; set; }
    public string? Country { get; set; }

    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public string? Website { get; set; }

    public ImageAuditSnapshot? HeroImage { get; set; }

    public List<TagAuditSnapshot> Tags { get; set; } = new();
    public List<KamiAuditSnapshot> Kami { get; set; } = new();
    public List<HistoryAuditSnapshot> Histories { get; set; } = new();
    public List<FolkloreAuditSnapshot> Folklores { get; set; } = new();
    public List<ImageAuditSnapshot> GalleryImages { get; set; } = new();
}

public class TagAuditSnapshot
{
    public int TagId { get; set; }
    public string? TitleEn { get; set; }
    public string? TitleJp { get; set; }
}

public class ImageAuditSnapshot
{
    public int ImgId { get; set; }
    public string? ImageUrl { get; set; }
    public string? Title { get; set; }
    public string? Desc { get; set; }
    public CitationAuditSnapshot? Citation { get; set; }
}

public class CitationAuditSnapshot
{
    public int CiteId { get; set; }
    public string? Title { get; set; }
    public string? Author { get; set; }
    public string? Url { get; set; }
    public int? Year { get; set; }
}

public class KamiAuditSnapshot
{
    public int KamiId { get; set; }
    public string? NameEn { get; set; }
    public string? NameJp { get; set; }
    public string? Desc { get; set; }
    public ImageAuditSnapshot? Image { get; set; }
    public List<CitationAuditSnapshot> Citations { get; set; } = new();
}

public class HistoryAuditSnapshot
{
    public int HistoryId { get; set; }
    public DateOnly? EventDate { get; set; }
    public int? SortOrder { get; set; }
    public string? Title { get; set; }
    public string? Information { get; set; }
    public ImageAuditSnapshot? Image { get; set; }
    public List<CitationAuditSnapshot> Citations { get; set; } = new();
}

public class FolkloreAuditSnapshot
{
    public int FolkloreId { get; set; }
    public int? SortOrder { get; set; }
    public string? Title { get; set; }
    public string? Information { get; set; }
    public ImageAuditSnapshot? Image { get; set; }
    public List<CitationAuditSnapshot> Citations { get; set; } = new();
}