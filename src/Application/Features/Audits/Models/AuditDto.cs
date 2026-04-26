using Application.Common.Models.Images;

namespace Application.Features.Audits.Models;

public record AuditDto(
    int AuditId,
    int UserId,
    string Username,
    string Action,
    string Target,
    bool IsSuccessful,
    string? Message,
    DateTime CreatedAt
);
