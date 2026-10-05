using Application.Features.Audits.Services;
using Application.Features.Kami.Services;
using MediatR;

namespace Application.Features.Kami.Commands.WithdrawDraftKami;

public class WithdrawDraftKamiHandler : IRequestHandler<WithdrawDraftKamiCommand, Unit>
{
    private readonly IKamiService _service;
    private readonly IAuditService _audit;

    public WithdrawDraftKamiHandler
    (
        IKamiService service,
        IAuditService audit
    )
    {
        _service = service;
        _audit = audit;
    }

    public async Task<Unit> Handle(WithdrawDraftKamiCommand request, CancellationToken ct)
    {
        try
        {
            await _service.WithdrawDraftKamiAsync(request.KamiId, request.UserId, ct);

            // Main Audit
            await _audit.LogAsync(request.UserId, request.Username, "WithdrewKamiFromReview", $"Kami #{request.KamiId} (Review)", true, null, ct);
        }
        catch (Exception e)
        {
            try
            {
                await _audit.LogAsync(request.UserId, request.Username, "WithdrewKamiFromReview", $"Kami #{request.KamiId} (Review)", false, e.Message, ct);
            }
            catch { }
            throw;
        }

        return Unit.Value;
    }
}