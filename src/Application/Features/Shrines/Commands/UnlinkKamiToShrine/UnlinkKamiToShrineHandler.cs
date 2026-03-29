using Application.Common.Policies;
using Application.Features.Shrines.Services;
using MediatR;

namespace Application.Features.Shrines.Commands.UnlinkKamiToShrine;

public class UnlinkKamiToShrineHandler : IRequestHandler<UnlinkKamiToShrineCommand, Unit>
{
    private readonly IShrineWriteService _shrineWriteService;
    private readonly IShrineReadService _shrineReadService;

    public UnlinkKamiToShrineHandler(IShrineWriteService shrineWriteService, IShrineReadService shrineReadService)
    {
        _shrineWriteService = shrineWriteService;
        _shrineReadService = shrineReadService;
    }

    public async Task<Unit> Handle(UnlinkKamiToShrineCommand request, CancellationToken ct)
    {
        // Validate Policy 
        var shrineStatus = await _shrineReadService.GetShrineStatusByIdCMSAsync(request.ShrineId, ct);
        ShrineWritePolicy.EnsureCanModify(shrineStatus, request.UserRole);

        // Unlink Kami To Shrine
        await _shrineWriteService.UnlinkKamiToShrineAsync(
            request.ShrineId,
            request.KamiId,
            ct
        );

        return Unit.Value;
    }
}