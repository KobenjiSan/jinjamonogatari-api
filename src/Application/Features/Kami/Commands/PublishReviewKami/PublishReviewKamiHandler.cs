using Application.Common.Exceptions;
using Application.Common.Services;
using Application.Features.Kami.Services;
using MediatR;

namespace Application.Features.Kami.Commands.PublishReviewKami;

public class PublishReviewKamiHandler : IRequestHandler<PublishReviewKamiCommand, Unit>
{
    private readonly IKamiService _service;
    private readonly IEntityAuditService _entityAudit;

    public PublishReviewKamiHandler
    (
        IKamiService service,
        IEntityAuditService entityAuditService
    )
    {
        _service = service;
        _entityAudit = entityAuditService;
    }

    public async Task<Unit> Handle(PublishReviewKamiCommand request, CancellationToken ct)
    {

        // Validate Kami has no errors
        var auditResult = await _entityAudit.AuditKamiAsync(request.KamiId, ct);

        if (auditResult.ErrorCount > 0)
            throw new BadRequestException("Publishing blocked due to audit errors.");

        await _service.PublishKamiForReviewAsync(request.KamiId, request.UserId, ct);

        return Unit.Value;
    }
}