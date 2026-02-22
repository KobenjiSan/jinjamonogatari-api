using Domain.Common;

namespace Domain.Entities;

public class ShrineGallery : IHasCreatedAt
{
    // Connected Tables
    public int ShrineId { get; set; }
    public Shrine Shrine { get; set; } = null!;

    public int ImgId { get; set; }
    public Image Image { get; set; } = null!;

    // Timestamp
    public DateTime CreatedAt { get; set; }
}