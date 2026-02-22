using Domain.Common;

namespace Domain.Entities;

public class HistoryCitation : IHasCreatedAt
{
    // Connected Tables
    public int HistoryId { get; set; }
    public History History { get; set; } = null!;

    public int CiteId { get; set; }
    public Citation Citation { get; set; } = null!;

    // Timestamp
    public DateTime CreatedAt { get; set; }
}