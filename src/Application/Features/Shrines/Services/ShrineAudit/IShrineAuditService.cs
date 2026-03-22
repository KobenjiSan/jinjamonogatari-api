using Application.Features.Shrines.Models;

namespace Application.Features.Shrines.Services.ShrineAudit;

public interface IShrineAuditService
{
    ShrineAuditDto Evaluate(ShrineAuditSnapshot snapshot);
    EntityAuditDto EvaluateKami(KamiAuditSnapshot kami);
    EntityAuditDto EvaluateFolklore(FolkloreAuditSnapshot folklore);
    EntityAuditDto EvaluateHistory(HistoryAuditSnapshot history);
    EntityAuditDto EvaluateGalleryImage(ImageAuditSnapshot image);
}