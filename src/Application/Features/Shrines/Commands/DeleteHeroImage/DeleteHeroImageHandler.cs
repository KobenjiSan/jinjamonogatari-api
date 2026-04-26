using Application.Common.Policies;
using Application.Features.Audits.Services;
using Application.Features.Images.Services;
using Application.Features.Shrines.Services;
using MediatR;

namespace Application.Features.Shrines.Commands.DeleteHeroImage;

public class DeleteHeroImageHandler : IRequestHandler<DeleteHeroImageCommand, Unit>
{
    private readonly IShrineWriteService _shrineWriteService;
    private readonly IShrineReadService _shrineReadService;
    private readonly IImageService _imageService;
    private readonly IAuditService _audit;

    public DeleteHeroImageHandler(
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

    public async Task<Unit> Handle(DeleteHeroImageCommand request, CancellationToken ct)
    {
        try
        {
            // Validate Policy
            var shrineStatus = await _shrineReadService.GetShrineStatusByIdCMSAsync(request.ShrineId, ct);
            ShrineWritePolicy.EnsureCanModify(shrineStatus, request.UserRole);

            // Remove Image from Cloudinary
            string? publicId = await _shrineReadService.GetImagePublicIdCMSAsync(request.ImageId, ct);
            if (!string.IsNullOrWhiteSpace(publicId)) await _imageService.DeleteAsync(publicId, ct);

            await _shrineWriteService.DeleteHeroImageAsync(
                request.ShrineId,
                request.ImageId,
                ct
            );

            await _audit.LogAsync(request.UserId, request.Username, "DeletedHeroImage", $"Shrine #{request.ShrineId} (Hero Image #{request.ImageId})", true, null, ct);
        }
        catch (Exception e)
        {
            try
            {
                await _audit.LogAsync(request.UserId, request.Username, "DeletedHeroImage", $"Shrine #{request.ShrineId} (Hero Image #{request.ImageId})", false, e.Message, ct);
            }
            catch { }
            throw;
        }

        return Unit.Value;
    }
}