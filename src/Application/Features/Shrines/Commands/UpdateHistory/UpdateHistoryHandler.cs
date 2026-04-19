using Application.Common.Policies;
using Application.Features.Images.Services;
using Application.Features.Shrines.Services;
using MediatR;

namespace Application.Features.Shrines.Commands.UpdateHistory;

public class UpdateHistoryHandler : IRequestHandler<UpdateHistoryCommand, Unit>
{
    private readonly IShrineWriteService _shrineWriteService;
    private readonly IShrineReadService _shrineReadService;
    private readonly IImageService _imageService;

    public UpdateHistoryHandler(
        IShrineWriteService shrineWriteService, 
        IShrineReadService shrineReadService,
        IImageService imageService
    )
    {
        _shrineWriteService = shrineWriteService;
        _shrineReadService = shrineReadService;
        _imageService = imageService;
    }

    public async Task<Unit> Handle(UpdateHistoryCommand request, CancellationToken ct)
    {
        // Validate Policy 
        var shrineId = await _shrineReadService.GetShrineIdByHistoryIdCMSAsync(request.HistoryId, ct);
        var shrineStatus = await _shrineReadService.GetShrineStatusByIdCMSAsync(shrineId, ct);
        ShrineWritePolicy.EnsureCanModify(shrineStatus, request.UserRole);

        var data = request.Request;
        var file = request.File;

        var finalData = data;
        string? publicId = null;

        if (data.Image is not null)
        {
            if (data.Image.Action == "delete")
            {
                var existingPublicId = await _shrineReadService.GetHistoryImagePublicIdCMSAsync(request.HistoryId, ct);

                if (!string.IsNullOrWhiteSpace(existingPublicId))
                    await _imageService.DeleteAsync(existingPublicId, ct);
            }
            else if (data.Image.Action == "create" || data.Image.Action == "update")
            {
                string? resolvedImageUrl = data.Image.ImageUrl;

                if (data.Image.Action == "update" && file is not null)
                {
                    var existingPublicId = await _shrineReadService.GetHistoryImagePublicIdCMSAsync(request.HistoryId, ct);

                    if (!string.IsNullOrWhiteSpace(existingPublicId))
                        await _imageService.DeleteAsync(existingPublicId, ct);
                }

                if (file is not null)
                {
                    var uploadResult = await _imageService.UploadAsync(file, $"jinja/shrines/{shrineId}/history", ct);
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
        await _shrineWriteService.UpdateHistoryAsync(
            request.HistoryId,
            finalData,
            publicId,
            ct
        );

        return Unit.Value;
    }
}