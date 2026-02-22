using Domain.Common;

namespace Domain.Entities;

public class Folklore : IHasTimestamps
{
    // PK
    public int FolkloreId { get; set; }

    // Connected Shrine
    public int ShrineId { get; set; }
    public Shrine Shrine { get; set; } = null!;

    // Content
    public int? SortOrder { get; set; }

    public string? Title { get; set; }
    public string? Information { get; set; }

    // Main image
    public int? ImgId { get; set; }
    public Image? Image { get; set; }

    // Publishing
    public string? Status { get; set; }

    // Timestamps
    public DateTime? PublishedAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Relationships (collections)
    public ICollection<FolkloreCitation> FolkloreCitations { get; set; } = new List<FolkloreCitation>();
}