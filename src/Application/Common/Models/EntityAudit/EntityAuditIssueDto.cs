namespace Application.Common.Models.EntityAudit;

public record EntityAuditIssueDto(
    int EntityAuditIssueId,
    int EntityAuditId,
    string Severity,
    string Field,
    string Message,
    string? RelatedItemType,
    int? RelatedItemId,
    DateTime CreatedAt
);  

public record EntityAuditIssueDraft(
    string Severity,
    string Field,
    string Message,
    string? RelatedItemType,
    int? RelatedItemId
);    