using Application.Common.Policies;
using Application.Features.Audits.Services;
using Application.Features.Shrines.Services;
using MediatR;

namespace Application.Features.Shrines.Commands.UpdateShrineNotes;

public class UpdateShrineNotesHandler : IRequestHandler<UpdateShrineNotesCommand, Unit>
{
    private readonly IShrineWriteService _shrineWriteService;
    private readonly IShrineReadService _shrineReadService;
    private readonly IAuditService _audit;

    public UpdateShrineNotesHandler(
        IShrineWriteService shrineWriteService,
        IShrineReadService shrineReadService,
        IAuditService audit
    )
    {
        _shrineWriteService = shrineWriteService;
        _shrineReadService = shrineReadService;
        _audit = audit;
    }

    public async Task<Unit> Handle(UpdateShrineNotesCommand request, CancellationToken ct)
    {
        try
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

            await _audit.LogAsync(request.UserId, request.Username, "UpdatedShrineNotes", $"Shrine #{request.ShrineId} (Notes)", true, null, ct);
        }
        catch (Exception e)
        {
            try
            {
                await _audit.LogAsync(request.UserId, request.Username, "UpdatedShrineNotes", $"Shrine #{request.ShrineId} (Notes)", false, e.Message, ct);
            }
            catch { }
            throw;
        }

        return Unit.Value;
    }
}