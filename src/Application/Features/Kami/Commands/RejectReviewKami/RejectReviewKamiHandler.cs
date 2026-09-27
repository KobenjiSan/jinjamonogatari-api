using Application.Features.Audits.Services;
using Application.Features.Kami.Services;
using MediatR;

namespace Application.Features.Kami.Commands.RejectReviewKami;

public class RejectReviewKamiHandler : IRequestHandler<RejectReviewKamiCommand, Unit>
{
    private readonly IKamiService _service;
    private readonly IAuditService _audit;

    public RejectReviewKamiHandler
    (
        IKamiService service,
        IAuditService audit
    )
    {
        _service = service;
        _audit = audit;
    }

    public async Task<Unit> Handle(RejectReviewKamiCommand request, CancellationToken ct)
    {
        try
        {
            await _service.RejectKamiForReviewAsync(request.KamiId, request.UserId, request.Message, ct);

            // Main Audit
            await _audit.LogAsync(request.UserId, request.Username, "RejectedKami", $"Kami #{request.KamiId} (Review)", true, null, ct);
        }
        catch (Exception e)
        {
            try
            {
                await _audit.LogAsync(request.UserId, request.Username, "RejectedKami", $"Kami #{request.KamiId} (Review)", false, e.Message, ct);
            }
            catch { }
            throw;
        }

        return Unit.Value;
    }
}