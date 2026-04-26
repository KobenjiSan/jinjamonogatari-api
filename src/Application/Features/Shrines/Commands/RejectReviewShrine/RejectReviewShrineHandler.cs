using Application.Features.Audits.Services;
using Application.Features.Shrines.Services;
using MediatR;

namespace Application.Features.Shrines.Commands.RejectReviewShrine;

public class RejectReviewShrineHandler : IRequestHandler<RejectReviewShrineCommand, Unit>
{
    private readonly IShrineWriteService _shrineWriteService;
    private readonly IAuditService _audit;

    public RejectReviewShrineHandler(
        IShrineWriteService shrineWriteService,
        IAuditService audit
    )
    {
        _shrineWriteService = shrineWriteService;
        _audit = audit;
    }

    public async Task<Unit> Handle(RejectReviewShrineCommand request, CancellationToken ct)
    {
        try
        {
            await _shrineWriteService.RejectShrineForReview(request.ShrineId, request.UserId, request.Message, ct);

            await _audit.LogAsync(request.UserId, request.Username, "RejectedShrine", $"Shrine #{request.ShrineId} (Review)", true, null, ct);
        }
        catch (Exception e)
        {
            try
            {
                await _audit.LogAsync(request.UserId, request.Username, "RejectedShrine", $"Shrine #{request.ShrineId} (Review)", false, e.Message, ct);
            }
            catch { }
            throw;
        }

        return Unit.Value;
    }
}