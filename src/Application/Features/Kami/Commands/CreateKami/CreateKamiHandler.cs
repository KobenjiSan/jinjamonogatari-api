using Application.Features.Images.Services;
using Application.Features.Kami.Services;
using MediatR;

namespace Application.Features.Kami.Commands.CreateKami;

public class CreateKamiHandler : IRequestHandler<CreateKamiCommand, Unit>
{
    private readonly IKamiService _service;
    private readonly IImageService _imageService;

    public CreateKamiHandler(
        IKamiService service,
        IImageService imageService
    )
    {
        _service = service;
        _imageService = imageService;
    }

    public async Task<Unit> Handle(CreateKamiCommand request, CancellationToken ct)
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
                var uploadResult = await _imageService.UploadAsync(file, "jinja/kami", ct);
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

        // Create Kami
        await _service.CreateKamiAsync(
            finalData,
            publicId,
            ct
        );

        return Unit.Value;
    }
}