using Application.Features.Audits.Models;
using Application.Features.Audits.Queries.GetAuditLog;

namespace Application.Features.Audits.Services;

public interface IAuditService
{
    Task LogAsync(int userId, string username, string action, string target, bool isSuccessful, string? message, CancellationToken ct);
    Task<(IReadOnlyList<AuditDto>, int)> GetAuditLogAsync(GetAuditLogQuery request, CancellationToken ct);
}