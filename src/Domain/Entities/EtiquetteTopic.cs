using Domain.Common;

namespace Domain.Entities;

public class EtiquetteTopic : IHasTimestamps
{
    // PK
    public int TopicId { get; set; }

    // Identifier
    public string Slug { get; set; } = null!;

    // Content
    public string? TitleLong { get; set; }
    public string? Summary { get; set; }

    // At a Glance
    public bool ShowInGlance { get; set; }
    public string? TitleShort { get; set; }     // max 12 chars
    public string? IconKey { get; set; }
    public string? IconSet { get; set; }
    public int? GlanceOrder { get; set; }

    // UI positioning
    public bool ShowAsHighlight { get; set; }
    public int? GuideOrder { get; set; }

    // Publishing
    public string? Status { get; set; }

    // Timestamps
    public DateTime? PublishedAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Relationships (collections)
    public ICollection<EtiquetteStep> Steps { get; set; } = new List<EtiquetteStep>();
    public ICollection<EtiquetteTopicCitation> TopicCitations { get; set; } = new List<EtiquetteTopicCitation>();
}