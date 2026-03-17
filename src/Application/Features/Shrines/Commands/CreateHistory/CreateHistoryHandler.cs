using Application.Features.Shrines.Services;
using MediatR;

namespace Application.Features.Shrines.Commands.CreateHistory;

public class CreateHistoryHandler : IRequestHandler<CreateHistoryCommand, Unit>
{
    private readonly IShrineWriteService _shrineWriteService;

    public CreateHistoryHandler(IShrineWriteService shrineWriteService)
    {
        _shrineWriteService = shrineWriteService;
    }

    public async Task<Unit> Handle(CreateHistoryCommand request, CancellationToken ct)
    {
        await _shrineWriteService.CreateHistoryAsync(
            request.ShrineId,
            request.Request,
            ct
        );

        return Unit.Value;
    }
}