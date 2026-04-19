using Application.Features.Images.Services;
using Application.Features.Kami.Services;
using MediatR;

namespace Application.Features.Kami.Commands.UpdateKami;

public class UpdateKamiHandler : IRequestHandler<UpdateKamiCommand, Unit>
{
    private readonly IKamiService _service;
    private readonly IImageService _imageService;

    public UpdateKamiHandler(
        IKamiService service,
        IImageService imageService
    )
    {
        _service = service;
        _imageService = imageService;
    }

    public async Task<Unit> Handle(UpdateKamiCommand request, CancellationToken ct)
    {
        var data = request.Request;
        var file = request.File;

        var finalData = data;
        string? publicId = null;

        if (data.Image is not null)
        {
            if (data.Image.Action == "delete")
            {
                var existingPublicId = await _service.GetKamiImagePublicIdCMSAsync(request.KamiId, ct);

                if (!string.IsNullOrWhiteSpace(existingPublicId))
                    await _imageService.DeleteAsync(existingPublicId, ct);
            }
            else if (data.Image.Action == "create" || data.Image.Action == "update")
            {
                string? resolvedImageUrl = data.Image.ImageUrl;

                if (data.Image.Action == "update" && file is not null)
                {
                    var existingPublicId = await _service.GetKamiImagePublicIdCMSAsync(request.KamiId, ct);

                    if (!string.IsNullOrWhiteSpace(existingPublicId))
                        await _imageService.DeleteAsync(existingPublicId, ct);
                }

                if (file is not null)
                {
                    var uploadResult = await _imageService.UploadAsync(file, "jinja/kami", ct);
                    resolvedImageUrl = uploadResult.Url;
                    publicId = uploadResult.PublicId;
                }

                if (string.IsNullOrWhiteSpace(resolvedImageUrl)) 
                    throw new ArgumentException("Either an image file or image URL is required.");

                var finalImage = data.Image with
                {
                    ImageUrl = resolvedImageUrl
                };

                finalData = data with
                {
                    Image = finalImage
                };
            }
        }

        await _service.UpdateKamiAsync(
            request.KamiId,
            finalData,
            publicId,
            ct
        );

        return Unit.Value;
    }
}