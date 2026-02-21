using Domain.Common;

namespace Domain.Entities;

public class Tag : IHasTimestamps
{
    public int TagId { get; set; }

    public string TitleEn { get; set; } = null!;
    public string? TitleJp { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Relationships (collections)
    public List<ShrineTag> ShrineTags { get; set; } = new();
}