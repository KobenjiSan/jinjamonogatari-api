namespace Application.Features.Shrines.Models;

public record EntityAuditDto(
    int? ItemId,
    int ErrorCount,
    int WarningCount,
    bool HasErrors,
    bool HasWarnings,
    IReadOnlyList<AuditIssueDto> Issues
);