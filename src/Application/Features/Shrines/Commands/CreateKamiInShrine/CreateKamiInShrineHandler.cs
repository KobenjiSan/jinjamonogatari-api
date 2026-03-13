using Application.Features.Shrines.Services;
using MediatR;

namespace Application.Features.Shrines.Commands.CreateKamiInShrine;

public class CreateKamiInShrineHandler : IRequestHandler<CreateKamiInShrineCommand, Unit>
{
    private readonly IShrineWriteService _shrineWriteService;

    public CreateKamiInShrineHandler(IShrineWriteService shrineWriteService)
    {
        _shrineWriteService = shrineWriteService;
    }

    public async Task<Unit> Handle(CreateKamiInShrineCommand request, CancellationToken ct)
    {
        await _shrineWriteService.CreateKamiInShrineAsync(
            request.ShrineId,
            request.Request,
            ct
        );

        return Unit.Value;
    }
}