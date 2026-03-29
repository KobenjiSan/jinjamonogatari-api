using Application.Common.Policies;
using Application.Features.Shrines.Services;
using MediatR;

namespace Application.Features.Shrines.Commands.DeleteHistory;

public class DeleteHistoryHandler : IRequestHandler<DeleteHistoryCommand, Unit>
{
    private readonly IShrineWriteService _shrineWriteService;
    private readonly IShrineReadService _shrineReadService;

    public DeleteHistoryHandler(IShrineWriteService shrineWriteService, IShrineReadService shrineReadService)
    {
        _shrineWriteService = shrineWriteService;
        _shrineReadService = shrineReadService;
    }

    public async Task<Unit> Handle(DeleteHistoryCommand request, CancellationToken ct)
    {
        // Validate Policy 
        var shrineId = await _shrineReadService.GetShrineIdByHistoryIdCMSAsync(request.HistoryId, ct);
        var shrineStatus = await _shrineReadService.GetShrineStatusByIdCMSAsync(shrineId, ct);
        ShrineWritePolicy.EnsureCanModify(shrineStatus, request.UserRole);

        // Delete Shrine History
        await _shrineWriteService.DeleteHistoryAsync(request.HistoryId, ct);

        return Unit.Value;
    }
}