using Application.Common.Policies;
using Application.Features.Shrines.Services;
using MediatR;

namespace Application.Features.Shrines.Commands.UpdateShrineNotes;

public class UpdateShrineNotesHandler : IRequestHandler<UpdateShrineNotesCommand, Unit>
{
    private readonly IShrineWriteService _shrineWriteService;
    private readonly IShrineReadService _shrineReadService;

    public UpdateShrineNotesHandler(IShrineWriteService shrineWriteService, IShrineReadService shrineReadService)
    {
        _shrineWriteService = shrineWriteService;
        _shrineReadService = shrineReadService;
    }

    public async Task<Unit> Handle(UpdateShrineNotesCommand request, CancellationToken ct)
    {
        // Validate Policy 
        var shrineStatus = await _shrineReadService.GetShrineStatusByIdCMSAsync(request.ShrineId, ct);
        ShrineWritePolicy.EnsureCanModify(shrineStatus, request.UserRole);

        // Update Shrine Notes
        await _shrineWriteService.UpdateShrineNotesAsync(
            request.ShrineId,
            request.Notes,
            ct
        );

        return Unit.Value;
    }
}