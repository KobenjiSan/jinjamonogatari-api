using Domain.Common;

namespace Domain.Entities;

public class History : IHasTimestamps
{
    // PK
    public int HistoryId { get; set; }

    // Connected Shrine
    public int ShrineId { get; set; }
    public Shrine Shrine { get; set; } = null!;

    // Content
    public DateOnly? EventDate { get; set; }
    public int? SortOrder { get; set; }

    public string? Title { get; set; }
    public string? Information { get; set; }

    // Hero Image
    public int? ImgId { get; set; }
    public Image? Image { get; set; }

    // Publishing
    public string? Status { get; set; }

    // Timestamps
    public DateTime? PublishedAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Relationships (collections)
    public ICollection<HistoryCitation> HistoryCitations { get; set; } = new List<HistoryCitation>();
}