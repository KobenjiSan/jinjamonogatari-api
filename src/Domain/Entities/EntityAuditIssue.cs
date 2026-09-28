using Domain.Common;

namespace Domain.Entities;

public class EntityAuditIssue : IHasCreatedAt
{
    // PK
    public int EntityAuditIssueId { get; set; }

    // Link to main audit
    public int EntityAuditId { get; set; }
    public EntityAudit EntityAudit  { get; set; } = null!;

    // Basic
    public string Severity { get; set; } = string.Empty;
    public string Field { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;

    // Reference to related item if needed (image or citation)
    public string? RelatedItemType { get; set; }
    public int? RelatedItemId { get; set; }

    // Timestamps
    public DateTime CreatedAt { get; set; }
}