using Application.Common.Policies;
using Application.Features.Images.Services;
using Application.Features.Shrines.Services;
using MediatR;

namespace Application.Features.Shrines.Commands.CreateKamiInShrine;

public class CreateKamiInShrineHandler : IRequestHandler<CreateKamiInShrineCommand, Unit>
{
    private readonly IShrineWriteService _shrineWriteService;
    private readonly IShrineReadService _shrineReadService;
    private readonly IImageService _imageService;

    public CreateKamiInShrineHandler(
        IShrineWriteService shrineWriteService, 
        IShrineReadService shrineReadService, 
        IImageService imageService
    )
    {
        _shrineWriteService = shrineWriteService;
        _shrineReadService = shrineReadService;
        _imageService = imageService;
    }

    public async Task<Unit> Handle(CreateKamiInShrineCommand request, CancellationToken ct)
    {
        // Validate Policy 
        var shrineStatus = await _shrineReadService.GetShrineStatusByIdCMSAsync(request.ShrineId, ct);
        ShrineWritePolicy.EnsureCanModify(shrineStatus, request.UserRole);

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
        await _shrineWriteService.CreateKamiInShrineAsync(
            request.ShrineId,
            finalData,
            publicId,
            ct
        );

        return Unit.Value;
    }
}