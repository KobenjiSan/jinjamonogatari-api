using Application.Features.Etiquette.Services;
using Application.Features.Images.Services;
using MediatR;

namespace Application.Features.Etiquette.Commands.CreateStep;

public class CreateStepHandler : IRequestHandler<CreateStepCommand, Unit>
{
    private readonly IEtiquetteWriteService _writeService;
    private readonly IImageService _imageService;

    public CreateStepHandler(
        IEtiquetteWriteService writeService,
        IImageService imageService
    )
    {
        _writeService = writeService;
        _imageService = imageService;
    }

    public async Task<Unit> Handle(CreateStepCommand request, CancellationToken ct)
    {
        var data = request.Request;
        var file = request.File;

        var finalData = data;
        string? publicId = null;

        if (data.Image is not null || file is not null)
        {
            string? resolvedImageUrl = data.Image?.ImageUrl;

            if (file is not null)
            {
                var uploadResult = await _imageService.UploadAsync(file, $"jinja/etiquette", ct);
                resolvedImageUrl = uploadResult.Url;
                publicId = uploadResult.PublicId;
            }

            if (string.IsNullOrWhiteSpace(resolvedImageUrl))
                throw new ArgumentException("Either an image file or image URL is required.");

            var finalImage = (data.Image ?? new CreateImageRequest
            (
                resolvedImageUrl,
                null,
                null,
                null
            )) with
            {
                ImageUrl = resolvedImageUrl
            };
            finalData = data with { Image = finalImage };
        }

        // Create Step
        await _writeService.CreateStepAsync(
            request.TopicId,
            finalData,
            publicId,
            ct
        );

        return Unit.Value;
    }
}