using Application.Common.Models.EntityAudit;

namespace Application.Common.Services;

public interface IEntityAuditService
{
    Task<EntityAuditCMSDto> AuditKamiAsync(int kamiId, CancellationToken ct);
}