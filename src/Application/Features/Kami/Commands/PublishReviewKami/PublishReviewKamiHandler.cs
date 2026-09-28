using Application.Common.Exceptions;
using Application.Common.Services;
using Application.Features.Audits.Services;
using Application.Features.Kami.Services;
using MediatR;

namespace Application.Features.Kami.Commands.PublishReviewKami;

public class PublishReviewKamiHandler : IRequestHandler<PublishReviewKamiCommand, Unit>
{
    private readonly IKamiService _service;
    private readonly IEntityAuditService _entityAudit;
    private readonly IAuditService _audit;

    public PublishReviewKamiHandler
    (
        IKamiService service,
        IEntityAuditService entityAuditService,
        IAuditService audit
    )
    {
        _service = service;
        _entityAudit = entityAuditService;
        _audit = audit;
    }

    public async Task<Unit> Handle(PublishReviewKamiCommand request, CancellationToken ct)
    {
        try
        {
            // Validate Kami has no errors
            var auditResult = await _entityAudit.AuditKamiAsync(request.KamiId, ct);

            if (auditResult.ErrorCount > 0)
                throw new BadRequestException("Publishing blocked due to audit errors.");

            await _service.PublishKamiForReviewAsync(request.KamiId, request.UserId, ct);

            // Main Audit
            await _audit.LogAsync(request.UserId, request.Username, "PublishedKami", $"Kami #{request.KamiId} (Review)", true, null, ct);
        }
        catch (Exception e)
        {
            try
            {
                await _audit.LogAsync(request.UserId, request.Username, "PublishedKami", $"Kami #{request.KamiId} (Review)", false, e.Message, ct);
            }
            catch { }
            throw;
        }

        return Unit.Value;
    }
}