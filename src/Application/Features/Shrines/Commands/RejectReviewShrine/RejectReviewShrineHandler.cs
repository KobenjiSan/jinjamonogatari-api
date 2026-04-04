using Application.Common.Exceptions;
using Application.Features.Shrines.Services;
using Application.Features.Shrines.Services.ShrineAudit;
using MediatR;

namespace Application.Features.Shrines.Commands.RejectReviewShrine;

public class RejectReviewShrineHandler : IRequestHandler<RejectReviewShrineCommand, Unit>
{
    private readonly IShrineWriteService _shrineWriteService;

    public RejectReviewShrineHandler(IShrineWriteService shrineWriteService)
    {
        _shrineWriteService = shrineWriteService;
    }

    public async Task<Unit> Handle(RejectReviewShrineCommand request, CancellationToken ct)
    {
        await _shrineWriteService.RejectShrineForReview(request.ShrineId, request.UserId, request.Message, ct);

        return Unit.Value;
    }
}