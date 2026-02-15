namespace Domain.Entities;

public class Image
{
    public int ImgId { get; set; }

    public string? ImgSource { get; set; }
    public string? Title { get; set; }
    public string? Desc { get; set; }

    public int? CiteId { get; set; }
    public Citation? Citation { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public List<EtiquetteTopic> EtiquetteTopicsAsHero { get; set; } = new();
    public List<EtiquetteStep> EtiquetteSteps { get; set; } = new();
}