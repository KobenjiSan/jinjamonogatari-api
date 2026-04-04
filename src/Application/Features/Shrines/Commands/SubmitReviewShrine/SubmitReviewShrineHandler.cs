using Application.Common.Exceptions;
using Application.Features.Shrines.Services;
using Application.Features.Shrines.Services.ShrineAudit;
using MediatR;

namespace Application.Features.Shrines.Commands.SubmitReviewShrine;

public class SubmitReviewShrineHandler : IRequestHandler<SubmitReviewShrineCommand, Unit>
{
    private readonly IShrineWriteService _shrineWriteService;
    private readonly IShrineReadService _shrineReadService;
    private readonly IShrineAuditService _shrineAuditService;

    public SubmitReviewShrineHandler
    (
        IShrineWriteService shrineWriteService, 
        IShrineReadService shrineReadService,
        IShrineAuditService shrineAuditService
    )
    {
        _shrineReadService = shrineReadService;
        _shrineAuditService = shrineAuditService;
        _shrineWriteService = shrineWriteService;
    }

    public async Task<Unit> Handle(SubmitReviewShrineCommand request, CancellationToken ct)
    {
        // Validate Shrine has no errors
        var snapshot = await _shrineReadService.GetShrineAuditSnapshotAsync(request.ShrineId, ct);

        if (snapshot is null)
            throw new NotFoundException($"Shrine {request.ShrineId} was not found.");

        var auditResult = _shrineAuditService.Evaluate(snapshot);

        if(auditResult.ErrorCount > 0)
            throw new BadRequestException("Submission blocked due to audit errors.");

        await _shrineWriteService.SubmitShrineForReview(request.ShrineId, request.UserId, ct);

        return Unit.Value;
    }
}