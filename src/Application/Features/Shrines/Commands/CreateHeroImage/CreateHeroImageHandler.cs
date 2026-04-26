using Application.Common.Models.Images;
using Application.Common.Policies;
using Application.Features.Audits.Services;
using Application.Features.Images.Services;
using Application.Features.Shrines.Services;
using MediatR;

namespace Application.Features.Shrines.Commands.CreateHeroImage;

public class CreateHeroImageHandler : IRequestHandler<CreateHeroImageCommand, ImageFullDto>
{
    private readonly IShrineWriteService _shrineWriteService;
    private readonly IShrineReadService _shrineReadService;
    private readonly IImageService _imageService;
    private readonly IAuditService _audit;

    public CreateHeroImageHandler(
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

    public async Task<ImageFullDto> Handle(CreateHeroImageCommand request, CancellationToken ct)
    {
        ImageFullDto result;

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
                    $"jinja/shrines/{request.ShrineId}/hero",
                    ct
                );

                resolvedImageUrl = uploadResult.Url;
                publicId = uploadResult.PublicId;
            }

            if (string.IsNullOrWhiteSpace(resolvedImageUrl))
                throw new ArgumentException("Either an image file or image URL is required.");

            var finalRequest = form with { ImageUrl = resolvedImageUrl };

            result = await _shrineWriteService.CreateHeroImageAsync(
                request.ShrineId,
                finalRequest,
                publicId,
                ct
            );

            await _audit.LogAsync(request.UserId, request.Username, "CreatedHeroImage", $"Shrine #{request.ShrineId} (Hero Image #{result.ImgId})", true, null, ct);
        }
        catch (Exception e)
        {
            try
            {
                await _audit.LogAsync(request.UserId, request.Username, "CreatedHeroImage", $"Shrine #{request.ShrineId}", false, e.Message, ct);
            }
            catch { }
            throw;
        }

        return result;
    }
}