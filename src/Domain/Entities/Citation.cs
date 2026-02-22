using Domain.Common;

namespace Domain.Entities;

public class Citation : IHasTimestamps
{
    // PK
    public int CiteId { get; set; }

    // Content
    public string? Title { get; set; }
    public string? Author { get; set; }
    public string? Url { get; set; }
    public int? Year { get; set; }

    // CMS viewable
    // Notes by Editors
    public string? Notes { get; set; }

    // Timestamps
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Relationships (collections)
    public ICollection<Image> ImagesAttributed { get; set; } = new List<Image>();
    public ICollection<EtiquetteTopicCitation> EtiquetteTopicCitations { get; set; } = new List<EtiquetteTopicCitation>();
    public ICollection<KamiCitation> KamiCitations { get; set; } = new List<KamiCitation>();
    public ICollection<HistoryCitation> HistoryCitations { get; set; } = new List<HistoryCitation>();
    public ICollection<FolkloreCitation> FolkloreCitations { get; set; } = new List<FolkloreCitation>();
}