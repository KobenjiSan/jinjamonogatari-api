namespace API.Domain.Entities;

public class Citation
{
    public int CiteId { get; set; }

    public string? Title { get; set; }
    public string? Author { get; set; }
    public string? Url { get; set; }
    public int? Year { get; set; }
    public string? Notes { get; set; }

    public List<EtiquetteTopicCitation> EtiquetteTopicCitations { get; set; } = new();
    public List<Image> ImagesAttributed { get; set; } = new();
}