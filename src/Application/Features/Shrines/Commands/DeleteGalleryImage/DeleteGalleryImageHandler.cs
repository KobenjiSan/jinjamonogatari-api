using Application.Common.Policies;
using Application.Features.Images.Services;
using Application.Features.Shrines.Services;
using MediatR;

namespace Application.Features.Shrines.Commands.DeleteGalleryImage;

public class DeleteGalleryImageHandler : IRequestHandler<DeleteGalleryImageCommand, Unit>
{
    private readonly IShrineWriteService _shrineWriteService;
    private readonly IShrineReadService _shrineReadService;
    private readonly IImageService _imageService;

    public DeleteGalleryImageHandler(IShrineWriteService shrineWriteService, IShrineReadService shrineReadService, IImageService imageService)
    {
        _shrineWriteService = shrineWriteService;
        _shrineReadService = shrineReadService;
        _imageService = imageService;
    }

    public async Task<Unit> Handle(DeleteGalleryImageCommand request, CancellationToken ct)
    {
        // Validate Policy 
        var shrineId = await _shrineReadService.GetShrineIdByImageIdCMSAsync(request.ImageId, ct);
        var shrineStatus = await _shrineReadService.GetShrineStatusByIdCMSAsync(shrineId, ct);
        ShrineWritePolicy.EnsureCanModify(shrineStatus, request.UserRole);

        // Remove Image from Cloudinary
        string? publicId = await _shrineReadService.GetImagePublicIdCMSAsync(request.ImageId, ct);
        if (!string.IsNullOrWhiteSpace(publicId)) await _imageService.DeleteAsync(publicId, ct);

        // Delete Gallery Image
        await _shrineWriteService.DeleteGalleryImageAsync(request.ImageId, ct);

        return Unit.Value;
    }
}