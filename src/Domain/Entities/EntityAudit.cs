using Domain.Common;

namespace Domain.Entities;

public class EntityAudit : IHasTimestamps
{
    // PK
    public int EntityAuditId { get; set; }

    // General Audit Stats
    public int ErrorCount { get; set; }
    public int WarningCount { get; set; }
    public bool CanSubmit { get; set; }

    // Timestamps
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // List of issues
    public ICollection<EntityAuditIssue> Issues { get; set; } = [];
}