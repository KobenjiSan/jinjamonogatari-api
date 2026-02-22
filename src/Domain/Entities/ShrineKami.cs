using Domain.Common;

namespace Domain.Entities;

public class ShrineKami : IHasCreatedAt
{
    // Connected Tables
    public int ShrineId { get; set; }
    public Shrine Shrine { get; set; } = null!;

    public int KamiId { get; set; }
    public Kami Kami { get; set; } = null!;

    // Timestamp
    public DateTime CreatedAt { get; set; }
}