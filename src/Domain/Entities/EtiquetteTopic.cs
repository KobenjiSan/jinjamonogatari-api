using Domain.Common;

namespace Domain.Entities;

public class EtiquetteTopic : IHasTimestamps
{
    public int TopicId { get; set; }

    public string Slug { get; set; } = null!;

    public string? TitleLong { get; set; }
    public string? TitleShort { get; set; }
    public string? Summary { get; set; }

    public string? IconKey { get; set; }
    public string? IconSet { get; set; }

    public int? ImageId { get; set; }
    public Image? Image { get; set; }

    public bool ShowInGlance { get; set; }
    public bool ShowAsHighlight { get; set; }
    public int? GlanceOrder { get; set; }
    public int? GuideOrder { get; set; }

    public string? Status { get; set; }
    public DateTime? PublishedAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public List<EtiquetteStep> Steps { get; set; } = new();
    public List<EtiquetteTopicCitation> TopicCitations { get; set; } = new();
}