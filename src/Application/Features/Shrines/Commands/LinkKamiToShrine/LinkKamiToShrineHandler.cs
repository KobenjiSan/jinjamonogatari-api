using Application.Common.Policies;
using Application.Features.Audits.Services;
using Application.Features.Shrines.Services;
using MediatR;

namespace Application.Features.Shrines.Commands.LinkKamiToShrine;

public class LinkKamiToShrineHandler : IRequestHandler<LinkKamiToShrineCommand, Unit>
{
    private readonly IShrineWriteService _shrineWriteService;
    private readonly IShrineReadService _shrineReadService;
    private readonly IAuditService _audit;

    public LinkKamiToShrineHandler(
        IShrineWriteService shrineWriteService,
        IShrineReadService shrineReadService,
        IAuditService audit
    )
    {
        _shrineWriteService = shrineWriteService;
        _shrineReadService = shrineReadService;
        _audit = audit;
    }

    public async Task<Unit> Handle(LinkKamiToShrineCommand request, CancellationToken ct)
    {
        try
        {
            // Validate Policy 
            var shrineStatus = await _shrineReadService.GetShrineStatusByIdCMSAsync(request.ShrineId, ct);
            ShrineWritePolicy.EnsureCanModify(shrineStatus, request.UserRole);

            // Link Kami To Shrine
            await _shrineWriteService.LinkKamiToShrineAsync(
                request.ShrineId,
                request.KamiId,
                ct
            );

            await _audit.LogAsync(request.UserId, request.Username, "LinkedKamiToShrine", $"Shrine #{request.ShrineId} (Kami #{request.KamiId})", true, null, ct);
        }
        catch (Exception e)
        {
            try
            {
                await _audit.LogAsync(request.UserId, request.Username, "LinkedKamiToShrine", $"Shrine #{request.ShrineId} (Kami #{request.KamiId})", false, e.Message, ct);
            }
            catch { }
            throw;
        }

        return Unit.Value;
    }
}