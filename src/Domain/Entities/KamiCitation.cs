using Domain.Common;

namespace Domain.Entities;

public class KamiCitation : IHasCreatedAt
{
    // Connected Tables
    public int KamiId { get; set; }
    public Kami Kami { get; set; } = null!;

    public int CiteId { get; set; }
    public Citation Citation { get; set; } = null!;

    // Timestamp
    public DateTime CreatedAt { get; set; }
}