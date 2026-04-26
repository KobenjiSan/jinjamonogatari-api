using Application.Common.Policies;
using Application.Features.Audits.Services;
using Application.Features.Shrines.Services;
using MediatR;

namespace Application.Features.Shrines.Commands.UpdateGalleryImage;

public class UpdateGalleryImageHandler : IRequestHandler<UpdateGalleryImageCommand, Unit>
{
    private readonly IShrineWriteService _shrineWriteService;
    private readonly IShrineReadService _shrineReadService;
    private readonly IAuditService _audit;

    public UpdateGalleryImageHandler(
        IShrineWriteService shrineWriteService,
        IShrineReadService shrineReadService,
        IAuditService audit
    )
    {
        _shrineWriteService = shrineWriteService;
        _shrineReadService = shrineReadService;
        _audit = audit;
    }

    public async Task<Unit> Handle(UpdateGalleryImageCommand request, CancellationToken ct)
    {
        int shrineId = 0;
        try
        {
            // Validate Policy 
            shrineId = await _shrineReadService.GetShrineIdByImageIdCMSAsync(request.ImageId, ct);
            var shrineStatus = await _shrineReadService.GetShrineStatusByIdCMSAsync(shrineId, ct);
            ShrineWritePolicy.EnsureCanModify(shrineStatus, request.UserRole);

            // Update Gallery Image
            await _shrineWriteService.UpdateGalleryImageAsync(
                request.ImageId,
                request.Request,
                ct
            );

            await _audit.LogAsync(request.UserId, request.Username, "UpdatedGalleryImage", $"Shrine #{shrineId} (Gallery Image #{request.ImageId})", true, null, ct);
        }
        catch (Exception e)
        {
            var target = shrineId != 0
                ? $"Shrine #{shrineId} (Gallery Image #{request.ImageId})"
                : $"Shrine -- (Gallery Image #{request.ImageId})";

            try
            {
                await _audit.LogAsync(request.UserId, request.Username, "UpdatedGalleryImage", target, false, e.Message, ct);
            }
            catch { }
            throw;
        }

        return Unit.Value;
    }
}