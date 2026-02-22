using Domain.Common;

namespace Domain.Entities;

public class FolkloreCitation : IHasCreatedAt
{
    // Connected Tables
    public int FolkloreId { get; set; }
    public Folklore Folklore { get; set; } = null!;

    public int CiteId { get; set; }
    public Citation Citation { get; set; } = null!;

    // Timestamp
    public DateTime CreatedAt { get; set; }
}