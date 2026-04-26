using Application.Common.Exceptions;
using Application.Features.Audits.Services;
using Application.Features.Shrines.Services;
using Application.Features.Shrines.Services.ShrineAudit;
using MediatR;

namespace Application.Features.Shrines.Commands.PublishReviewShrine;

public class PublishReviewShrineHandler : IRequestHandler<PublishReviewShrineCommand, Unit>
{
    private readonly IShrineWriteService _shrineWriteService;
    private readonly IShrineReadService _shrineReadService;
    private readonly IShrineAuditService _shrineAuditService;
    private readonly IAuditService _audit;

    public PublishReviewShrineHandler
    (
        IShrineWriteService shrineWriteService,
        IShrineReadService shrineReadService,
        IShrineAuditService shrineAuditService,
        IAuditService audit
    )
    {
        _shrineReadService = shrineReadService;
        _shrineAuditService = shrineAuditService;
        _shrineWriteService = shrineWriteService;
        _audit = audit;
    }

    public async Task<Unit> Handle(PublishReviewShrineCommand request, CancellationToken ct)
    {
        try
        {
            // Validate Shrine has no errors
            var snapshot = await _shrineReadService.GetShrineAuditSnapshotAsync(request.ShrineId, ct);

            if (snapshot is null)
                throw new NotFoundException($"Shrine {request.ShrineId} was not found.");

            var auditResult = _shrineAuditService.Evaluate(snapshot);

            if (auditResult.ErrorCount > 0)
                throw new BadRequestException("Publishing blocked due to audit errors.");

            await _shrineWriteService.PublishShrineForReview(request.ShrineId, request.UserId, ct);

            await _audit.LogAsync(request.UserId, request.Username, "PublishedShrine", $"Shrine #{request.ShrineId} (Review)", true, null, ct);
        }
        catch (Exception e)
        {
            try
            {
                await _audit.LogAsync(request.UserId, request.Username, "PublishedShrine", $"Shrine #{request.ShrineId} (Review)", false, e.Message, ct);
            }
            catch { }
            throw;
        }
        return Unit.Value;
    }
}