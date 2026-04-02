using Application.Features.Shrines.Services;
using MediatR;

namespace Application.Features.Shrines.Commands.DeleteShrine;

public class DeleteShrineHandler : IRequestHandler<DeleteShrineCommand, Unit>
{
    private readonly IShrineWriteService _shrineWriteService;

    public DeleteShrineHandler(IShrineWriteService shrineWriteService)
    {
        _shrineWriteService = shrineWriteService;
    }

    public async Task<Unit> Handle(DeleteShrineCommand request, CancellationToken ct)
    {
        await _shrineWriteService.DeleteShrineAsync(request.ShrineId, ct);

        return Unit.Value;
    }
}