using Application.Features.Kami.Services;
using MediatR;

namespace Application.Features.Kami.Commands.RejectReviewKami;

public class RejectReviewKamiHandler : IRequestHandler<RejectReviewKamiCommand, Unit>
{
    private readonly IKamiService _service;

    public RejectReviewKamiHandler(
        IKamiService service
    )
    {
        _service = service; ;
    }

    public async Task<Unit> Handle(RejectReviewKamiCommand request, CancellationToken ct)
    {
        await _service.RejectKamiForReviewAsync(request.KamiId, request.UserId, request.Message, ct);
        return Unit.Value;
    }
}