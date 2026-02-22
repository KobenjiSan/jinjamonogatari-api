using Domain.Common;

namespace Domain.Entities;

public class Image : IHasTimestamps
{
    // PK
    public int ImgId { get; set; }

    // Content
    public string? ImgSource { get; set; }
    public string? Title { get; set; }
    public string? Desc { get; set; }

    // Citation
    public int? CiteId { get; set; }
    public Citation? Citation { get; set; }

    // Timestamps
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}