using Application.Common.Policies;
using Application.Features.Shrines.Services;
using MediatR;

namespace Application.Features.Shrines.Commands.UpdateFolklore;

public class UpdateFolkloreHandler : IRequestHandler<UpdateFolkloreCommand, Unit>
{
    private readonly IShrineWriteService _shrineWriteService;
    private readonly IShrineReadService _shrineReadService;

    public UpdateFolkloreHandler(IShrineWriteService shrineWriteService, IShrineReadService shrineReadService)
    {
        _shrineWriteService = shrineWriteService;
        _shrineReadService = shrineReadService;
    }

    public async Task<Unit> Handle(UpdateFolkloreCommand request, CancellationToken ct)
    {
        // Validate Policy 
        var shrineId = await _shrineReadService.GetShrineIdByFolkloreIdCMSAsync(request.FolkloreId, ct);
        var shrineStatus = await _shrineReadService.GetShrineStatusByIdCMSAsync(shrineId, ct);
        ShrineWritePolicy.EnsureCanModify(shrineStatus, request.UserRole);

        // Update Shrine History
        await _shrineWriteService.UpdateFolkloreAsync(
            request.FolkloreId,
            request.Request,
            ct
        );

        return Unit.Value;
    }
}