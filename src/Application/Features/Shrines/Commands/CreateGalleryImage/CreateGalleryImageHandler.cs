using Application.Common.Policies;
using Application.Features.Audits.Services;
using Application.Features.Images.Services;
using Application.Features.Shrines.Services;
using MediatR;

namespace Application.Features.Shrines.Commands.CreateGalleryImage;

public class CreateGalleryImageHandler : IRequestHandler<CreateGalleryImageCommand, Unit>
{
    private readonly IShrineWriteService _shrineWriteService;
    private readonly IShrineReadService _shrineReadService;
    private readonly IImageService _imageService;
    private readonly IAuditService _audit;

    public CreateGalleryImageHandler(
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

    public async Task<Unit> Handle(CreateGalleryImageCommand request, CancellationToken ct)
    {
        try
        {
            // Validate Policy 
            var shrineStatus = await _shrineReadService.GetShrineStatusByIdCMSAsync(request.ShrineId, ct);
            ShrineWritePolicy.EnsureCanModify(shrineStatus, request.UserRole);

            var form = request.Request;
            string? resolvedImageUrl = form.ImageUrl;
            string? publicId = "";

            if (form.File is not null)
            {
                var uploadResult = await _imageService.UploadAsync(
                    form.File,
                    $"jinja/shrines/{request.ShrineId}/gallery",
                    ct
                );

                resolvedImageUrl = uploadResult.Url;
                publicId = uploadResult.PublicId;
            }

            if (string.IsNullOrWhiteSpace(resolvedImageUrl))
                throw new ArgumentException("Either an image file or image URL is required.");

            var finalRequest = form with { ImageUrl = resolvedImageUrl };

            // Create Gallery Image
            var imageId = await _shrineWriteService.CreateGalleryImageAsync(
                request.ShrineId,
                finalRequest,
                publicId,
                ct
            );

            await _audit.LogAsync(request.UserId, request.Username, "CreatedGalleryImage", $"Shrine #{request.ShrineId} (Gallery Image #{imageId})", true, null, ct);
        }
        catch (Exception e)
        {
            try
            {
                await _audit.LogAsync(request.UserId, request.Username, "CreatedGalleryImage", $"Shrine #{request.ShrineId}", false, e.Message, ct);
            }
            catch { }
            throw;
        }

        return Unit.Value;
    }
}