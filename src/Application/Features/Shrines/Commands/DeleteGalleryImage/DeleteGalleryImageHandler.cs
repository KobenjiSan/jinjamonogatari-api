using Application.Common.Policies;
using Application.Features.Audits.Services;
using Application.Features.Images.Services;
using Application.Features.Shrines.Services;
using MediatR;

namespace Application.Features.Shrines.Commands.DeleteGalleryImage;

public class DeleteGalleryImageHandler : IRequestHandler<DeleteGalleryImageCommand, Unit>
{
    private readonly IShrineWriteService _shrineWriteService;
    private readonly IShrineReadService _shrineReadService;
    private readonly IImageService _imageService;
    private readonly IAuditService _audit;

    public DeleteGalleryImageHandler(
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

    public async Task<Unit> Handle(DeleteGalleryImageCommand request, CancellationToken ct)
    {
        int shrineId = 0;
        try
        {
            // Validate Policy 
            shrineId = await _shrineReadService.GetShrineIdByImageIdCMSAsync(request.ImageId, ct);
            var shrineStatus = await _shrineReadService.GetShrineStatusByIdCMSAsync(shrineId, ct);
            ShrineWritePolicy.EnsureCanModify(shrineStatus, request.UserRole);

            // Remove Image from Cloudinary
            string? publicId = await _shrineReadService.GetImagePublicIdCMSAsync(request.ImageId, ct);
            if (!string.IsNullOrWhiteSpace(publicId)) await _imageService.DeleteAsync(publicId, ct);

            // Delete Gallery Image
            await _shrineWriteService.DeleteGalleryImageAsync(request.ImageId, ct);

            await _audit.LogAsync(request.UserId, request.Username, "DeletedGalleryImage", $"Shrine #{shrineId} (Gallery Image #{request.ImageId})", true, null, ct);
        }
        catch (Exception e)
        {
            var target = shrineId != 0
                ? $"Shrine #{shrineId} (Gallery Image #{request.ImageId})"
                : $"Shrine -- (Gallery Image #{request.ImageId})";

            try
            {
                await _audit.LogAsync(request.UserId, request.Username, "DeletedGalleryImage", target, false, e.Message, ct);
            }
            catch { }
            throw;
        }

        return Unit.Value;
    }
}