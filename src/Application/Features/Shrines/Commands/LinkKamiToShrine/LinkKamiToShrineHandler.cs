using Application.Common.Policies;
using Application.Features.Shrines.Services;
using MediatR;

namespace Application.Features.Shrines.Commands.LinkKamiToShrine;

public class LinkKamiToShrineHandler : IRequestHandler<LinkKamiToShrineCommand, Unit>
{
    private readonly IShrineWriteService _shrineWriteService;
    private readonly IShrineReadService _shrineReadService;

    public LinkKamiToShrineHandler(IShrineWriteService shrineWriteService, IShrineReadService shrineReadService)
    {
        _shrineWriteService = shrineWriteService;
        _shrineReadService = shrineReadService;
    }

    public async Task<Unit> Handle(LinkKamiToShrineCommand request, CancellationToken ct)
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

        return Unit.Value;
    }
}