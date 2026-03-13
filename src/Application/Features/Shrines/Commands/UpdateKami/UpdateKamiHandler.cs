using Application.Features.Shrines.Services;
using MediatR;

namespace Application.Features.Shrines.Commands.UpdateKami;

public class UpdateKamiHandler : IRequestHandler<UpdateKamiCommand, Unit>
{
    private readonly IShrineWriteService _shrineWriteService;

    public UpdateKamiHandler(IShrineWriteService shrineWriteService)
    {
        _shrineWriteService = shrineWriteService;
    }

    public async Task<Unit> Handle(UpdateKamiCommand request, CancellationToken ct)
    {
        await _shrineWriteService.UpdateKamiAsync(
            request.KamiId,
            request.Request,
            ct
        );

        return Unit.Value;
    }
}