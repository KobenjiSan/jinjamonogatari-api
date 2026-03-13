using Application.Features.Shrines.Services;
using MediatR;

namespace Application.Features.Shrines.Commands.LinkKamiToShrine;

public class LinkKamiToShrineHandler : IRequestHandler<LinkKamiToShrineCommand, Unit>
{
    private readonly IShrineWriteService _shrineWriteService;

    public LinkKamiToShrineHandler(IShrineWriteService shrineWriteService)
    {
        _shrineWriteService = shrineWriteService;
    }

    public async Task<Unit> Handle(LinkKamiToShrineCommand request, CancellationToken ct)
    {
        await _shrineWriteService.LinkKamiToShrineAsync(
            request.ShrineId,
            request.KamiId,
            ct
        );

        return Unit.Value;
    }
}