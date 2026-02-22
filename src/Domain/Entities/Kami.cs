using Domain.Common;

namespace Domain.Entities;

public class Kami : IHasTimestamps
{
    // PK
    public int KamiId { get; set; }

    // Content
    public string? NameEn { get; set; }
    public string? NameJp { get; set; }

    // Hero Image
    public int? ImgId { get; set; }
    public Image? Image { get; set; }

    public string? Desc { get; set; }

    // Publishing
    public string? Status { get; set; }

    // Timestamps
    public DateTime? PublishedAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Relationships (collections)
    public ICollection<ShrineKami> ShrineKamis { get; set; } = new List<ShrineKami>();
    public ICollection<KamiCitation> KamiCitations { get; set; } = new List<KamiCitation>();
}