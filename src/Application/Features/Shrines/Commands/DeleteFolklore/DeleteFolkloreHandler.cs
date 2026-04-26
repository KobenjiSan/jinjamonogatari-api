using Application.Common.Policies;
using Application.Features.Audits.Services;
using Application.Features.Images.Services;
using Application.Features.Shrines.Services;
using MediatR;

namespace Application.Features.Shrines.Commands.DeleteFolklore;

public class DeleteFolkloreHandler : IRequestHandler<DeleteFolkloreCommand, Unit>
{
    private readonly IShrineWriteService _shrineWriteService;
    private readonly IShrineReadService _shrineReadService;
    private readonly IImageService _imageService;
    private readonly IAuditService _audit;

    public DeleteFolkloreHandler(
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

    public async Task<Unit> Handle(DeleteFolkloreCommand request, CancellationToken ct)
    {
        int shrineId = 0;
        try
        {
            // Validate Policy 
            shrineId = await _shrineReadService.GetShrineIdByFolkloreIdCMSAsync(request.FolkloreId, ct);
            var shrineStatus = await _shrineReadService.GetShrineStatusByIdCMSAsync(shrineId, ct);
            ShrineWritePolicy.EnsureCanModify(shrineStatus, request.UserRole);

            // Remove Image from Cloudinary
            string? publicId = await _shrineReadService.GetFolkloreImagePublicIdCMSAsync(request.FolkloreId, ct);
            if (!string.IsNullOrWhiteSpace(publicId)) await _imageService.DeleteAsync(publicId, ct);

            // Delete Shrine Folklore
            await _shrineWriteService.DeleteFolkloreAsync(request.FolkloreId, ct);
            
            await _audit.LogAsync(request.UserId, request.Username, "DeletedFolklore", $"Shrine #{shrineId} (Folklore #{request.FolkloreId})", true, null, ct);
        }
        catch (Exception e)
        {
            var target = shrineId != 0
                ? $"Shrine #{shrineId} (Folklore #{request.FolkloreId})"
                : $"Shrine -- (Folklore #{request.FolkloreId})";

            try
            {
                await _audit.LogAsync(request.UserId, request.Username, "DeletedFolklore", target, false, e.Message, ct);
            }
            catch { }
            throw;
        }

        return Unit.Value;
    }
}