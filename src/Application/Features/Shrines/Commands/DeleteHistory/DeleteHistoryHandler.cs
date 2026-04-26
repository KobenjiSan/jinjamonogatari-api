using Application.Common.Policies;
using Application.Features.Audits.Services;
using Application.Features.Images.Services;
using Application.Features.Shrines.Services;
using MediatR;

namespace Application.Features.Shrines.Commands.DeleteHistory;

public class DeleteHistoryHandler : IRequestHandler<DeleteHistoryCommand, Unit>
{
    private readonly IShrineWriteService _shrineWriteService;
    private readonly IShrineReadService _shrineReadService;
    private readonly IImageService _imageService;
    private readonly IAuditService _audit;

    public DeleteHistoryHandler(
        IShrineWriteService shrineWriteService,
        IShrineReadService shrineReadService,
        IImageService imageService,
        IAuditService audit
    )
    {
        _shrineWriteService = shrineWriteService;
        _shrineReadService = shrineReadService;
        _imageService = imageService;
        _audit = audit;
    }

    public async Task<Unit> Handle(DeleteHistoryCommand request, CancellationToken ct)
    {
        int shrineId = 0;
        try
        {
            // Validate Policy 
            shrineId = await _shrineReadService.GetShrineIdByHistoryIdCMSAsync(request.HistoryId, ct);
            var shrineStatus = await _shrineReadService.GetShrineStatusByIdCMSAsync(shrineId, ct);
            ShrineWritePolicy.EnsureCanModify(shrineStatus, request.UserRole);

            // Remove Image from Cloudinary
            string? publicId = await _shrineReadService.GetHistoryImagePublicIdCMSAsync(request.HistoryId, ct);
            if (!string.IsNullOrWhiteSpace(publicId)) await _imageService.DeleteAsync(publicId, ct);

            // Delete Shrine History
            await _shrineWriteService.DeleteHistoryAsync(request.HistoryId, ct);

            await _audit.LogAsync(request.UserId, request.Username, "DeletedHistory", $"Shrine #{shrineId} (History #{request.HistoryId})", true, null, ct);
        }
        catch (Exception e)
        {
            var target = shrineId != 0
                ? $"Shrine #{shrineId} (History #{request.HistoryId})"
                : $"Shrine -- (History #{request.HistoryId})";

            try
            {
                await _audit.LogAsync(request.UserId, request.Username, "DeletedHistory", target, false, e.Message, ct);
            }
            catch { }
            throw;
        }

        return Unit.Value;
    }
}