using Application.Common.Policies;
using Application.Features.Images.Services;
using Application.Features.Shrines.Services;
using MediatR;

namespace Application.Features.Shrines.Commands.DeleteFolklore;

public class DeleteFolkloreHandler : IRequestHandler<DeleteFolkloreCommand, Unit>
{
    private readonly IShrineWriteService _shrineWriteService;
    private readonly IShrineReadService _shrineReadService;
    private readonly IImageService _imageService;

    public DeleteFolkloreHandler(
        IShrineWriteService shrineWriteService, 
        IShrineReadService shrineReadService,
        IImageService imageService
    )
    {
        _shrineWriteService = shrineWriteService;
        _shrineReadService = shrineReadService;
        _imageService = imageService;
    }

    public async Task<Unit> Handle(DeleteFolkloreCommand request, CancellationToken ct)
    {
        // Validate Policy 
        var shrineId = await _shrineReadService.GetShrineIdByFolkloreIdCMSAsync(request.FolkloreId, ct);
        var shrineStatus = await _shrineReadService.GetShrineStatusByIdCMSAsync(shrineId, ct);
        ShrineWritePolicy.EnsureCanModify(shrineStatus, request.UserRole);

        // Remove Image from Cloudinary
        string? publicId = await _shrineReadService.GetFolkloreImagePublicIdCMSAsync(request.FolkloreId, ct);
        if (!string.IsNullOrWhiteSpace(publicId)) await _imageService.DeleteAsync(publicId, ct);

        // Delete Shrine Folklore
        await _shrineWriteService.DeleteFolkloreAsync(request.FolkloreId, ct);

        return Unit.Value;
    }
}