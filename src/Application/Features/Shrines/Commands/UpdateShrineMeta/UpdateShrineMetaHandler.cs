using Application.Common.Policies;
using Application.Features.Audits.Services;
using Application.Features.Shrines.Services;
using MediatR;

namespace Application.Features.Shrines.Commands.UpdateShrineMeta;

public class UpdateShrineMetaHandler : IRequestHandler<UpdateShrineMetaCommand, Unit>
{
    private readonly IShrineWriteService _shrineWriteService;
    private readonly IShrineReadService _shrineReadService;
    private readonly IAuditService _audit;

    public UpdateShrineMetaHandler(
        IShrineWriteService shrineWriteService,
        IShrineReadService shrineReadService,
        IAuditService audit
    )
    {
        _shrineWriteService = shrineWriteService;
        _shrineReadService = shrineReadService;
        _audit = audit;
    }

    public async Task<Unit> Handle(UpdateShrineMetaCommand request, CancellationToken ct)
    {
        try
        {
            // Validate Policy 
            var shrineStatus = await _shrineReadService.GetShrineStatusByIdCMSAsync(request.ShrineId, ct);
            ShrineWritePolicy.EnsureCanModify(shrineStatus, request.UserRole);

            // Update Shrine Meta
            await _shrineWriteService.UpdateShrineMetaAsync(
                request.ShrineId,
                request.Request,
                ct
            );

            await _audit.LogAsync(request.UserId, request.Username, "UpdatedShrineMeta", $"Shrine #{request.ShrineId} (Meta)", true, null, ct);
        }
        catch (Exception e)
        {
            try
            {
                await _audit.LogAsync(request.UserId, request.Username, "UpdatedShrineMeta", $"Shrine #{request.ShrineId} (Meta)", false, e.Message, ct);
            }
            catch { }
            throw;
        }

        return Unit.Value;
    }
}