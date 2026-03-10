using Application.Features.Shrines.Services;
using MediatR;

namespace Application.Features.Shrines.Commands.UpdateShrineMeta;

public class UpdateShrineMetaHandler : IRequestHandler<UpdateShrineMetaCommand, Unit>
{
    private readonly IShrineWriteService _shrineWriteService;

    public UpdateShrineMetaHandler(IShrineWriteService shrineWriteService)
    {
        _shrineWriteService = shrineWriteService;
    }

    public async Task<Unit> Handle(UpdateShrineMetaCommand request, CancellationToken ct)
    {
        await _shrineWriteService.UpdateShrineMetaAsync(
            request.ShrineId,
            request.Request,
            ct
        );

        return Unit.Value;
    }
}