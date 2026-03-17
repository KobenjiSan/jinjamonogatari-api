using Application.Features.Shrines.Services;
using MediatR;

namespace Application.Features.Shrines.Commands.DeleteHistory;

public class DeleteHistoryHandler : IRequestHandler<DeleteHistoryCommand, Unit>
{
    private readonly IShrineWriteService _shrineWriteService;

    public DeleteHistoryHandler(IShrineWriteService shrineWriteService)
    {
        _shrineWriteService = shrineWriteService;
    }

    public async Task<Unit> Handle(DeleteHistoryCommand request, CancellationToken ct)
    {
        await _shrineWriteService.DeleteHistoryAsync(request.HistoryId, ct);

        return Unit.Value;
    }
}