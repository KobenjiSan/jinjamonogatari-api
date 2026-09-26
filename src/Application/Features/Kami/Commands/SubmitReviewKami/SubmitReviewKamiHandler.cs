using Application.Common.Exceptions;
using Application.Common.Services;
using Application.Features.Kami.Services;
using MediatR;

namespace Application.Features.Kami.Commands.SubmitReviewKami;

public class SubmitReviewKamiHandler : IRequestHandler<SubmitReviewKamiCommand, Unit>
{
    private readonly IKamiService _service;
    private readonly IEntityAuditService _entityAudit;

    public SubmitReviewKamiHandler
    (
        IKamiService service,
        IEntityAuditService entityAudit
    )
    {
        _service = service;
        _entityAudit = entityAudit;
    }

    public async Task<Unit> Handle(SubmitReviewKamiCommand request, CancellationToken ct)
    {

        // Validate Kami has no errors
        var auditResult = await _entityAudit.AuditKamiAsync(request.KamiId, ct);

        if (auditResult.ErrorCount > 0)
            throw new BadRequestException("Submission blocked due to audit errors.");

        await _service.SubmitKamiForReviewAsync(request.KamiId, request.UserId, ct);

        return Unit.Value;
    }
}