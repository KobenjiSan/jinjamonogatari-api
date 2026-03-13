using Application.Features.Shrines.Services;
using MediatR;

namespace Application.Features.Shrines.Commands.UnlinkKamiToShrine;

public class UnlinkKamiToShrineHandler : IRequestHandler<UnlinkKamiToShrineCommand, Unit>
{
    private readonly IShrineWriteService _shrineWriteService;

    public UnlinkKamiToShrineHandler(IShrineWriteService shrineWriteService)
    {
        _shrineWriteService = shrineWriteService;
    }

    public async Task<Unit> Handle(UnlinkKamiToShrineCommand request, CancellationToken ct)
    {
        await _shrineWriteService.UnlinkKamiToShrineAsync(
            request.ShrineId,
            request.KamiId,
            ct
        );

        return Unit.Value;
    }
}