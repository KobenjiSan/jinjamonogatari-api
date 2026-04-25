using Application.Features.Etiquette.Services;
using Application.Features.Images.Services;
using MediatR;

namespace Application.Features.Etiquette.Commands.UpdateStep;

public class UpdateStepHandler : IRequestHandler<UpdateStepCommand, Unit>
{
    private readonly IEtiquetteWriteService _writeService;
    private readonly IEtiquetteReadService _readService;
    private readonly IImageService _imageService;

    public UpdateStepHandler(
        IEtiquetteWriteService writeService, 
        IEtiquetteReadService readService,
        IImageService imageService
    )
    {
        _writeService = writeService;
        _readService = readService;
        _imageService = imageService;
    }

    public async Task<Unit> Handle(UpdateStepCommand request, CancellationToken ct)
    {
        var data = request.Request;
        var file = request.File;

        var finalData = data;
        string? publicId = null;

        if (data.Image is not null)
        {
            if (data.Image.Action == "delete")
            {
                var existingPublicId = await _readService.GetStepImagePublicIdCMSAsync(request.StepId, ct);

                if (!string.IsNullOrWhiteSpace(existingPublicId))
                    await _imageService.DeleteAsync(existingPublicId, ct);
            }
            else if (data.Image.Action == "create" || data.Image.Action == "update")
            {
                string? resolvedImageUrl = data.Image.ImageUrl;

                if (data.Image.Action == "update" && file is not null)
                {
                    var existingPublicId = await _readService.GetStepImagePublicIdCMSAsync(request.StepId, ct);

                    if (!string.IsNullOrWhiteSpace(existingPublicId))
                        await _imageService.DeleteAsync(existingPublicId, ct);
                }

                if (file is not null)
                {
                    var uploadResult = await _imageService.UploadAsync(file, $"jinja/etiquette", ct);
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

        // Update Shrine History
        await _writeService.UpdateStepAsync(
            request.StepId,
            finalData,
            publicId,
            ct
        );

        return Unit.Value;
    }
}