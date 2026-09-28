namespace Application.Common.Models.EntityAudit;

public record EntityAuditCMSDto(
    int EntityAuditId,
    int ErrorCount,
    int WarningCount,
    bool CanSubmit,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    IReadOnlyList<EntityAuditIssueDto> Issues
);