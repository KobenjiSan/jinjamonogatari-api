using Application.Features.Shrines.Services;
using MediatR;

namespace Application.Features.Shrines.Commands.UpdateHistory;

public class UpdateHistoryHandler : IRequestHandler<UpdateHistoryCommand, Unit>
{
    private readonly IShrineWriteService _shrineWriteService;

    public UpdateHistoryHandler(IShrineWriteService shrineWriteService)
    {
        _shrineWriteService = shrineWriteService;
    }

    public async Task<Unit> Handle(UpdateHistoryCommand request, CancellationToken ct)
    {
        await _shrineWriteService.UpdateHistoryAsync(
            request.HistoryId,
            request.Request,
            ct
        );

        return Unit.Value;
    }
}