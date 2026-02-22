using Domain.Common;

namespace Domain.Entities;

public class Tag : IHasTimestamps
{
    // PK
    public int TagId { get; set; }

    // Content
    public string TitleEn { get; set; } = null!;
    public string? TitleJp { get; set; }

    // Timestamps
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Relationships (collections)
    public ICollection<ShrineTag> ShrineTags { get; set; } = new List<ShrineTag>();
}