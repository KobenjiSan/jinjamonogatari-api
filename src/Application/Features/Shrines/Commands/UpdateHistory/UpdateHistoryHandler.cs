using Application.Common.Policies;
using Application.Features.Audits.Services;
using Application.Features.Images.Services;
using Application.Features.Shrines.Services;
using MediatR;

namespace Application.Features.Shrines.Commands.UpdateHistory;

public class UpdateHistoryHandler : IRequestHandler<UpdateHistoryCommand, Unit>
{
    private readonly IShrineWriteService _shrineWriteService;
    private readonly IShrineReadService _shrineReadService;
    private readonly IImageService _imageService;
    private readonly IAuditService _audit;

    public UpdateHistoryHandler(
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

    public async Task<Unit> Handle(UpdateHistoryCommand request, CancellationToken ct)
    {
        int shrineId = 0;
        try
        {
            // Validate Policy 
            shrineId = await _shrineReadService.GetShrineIdByHistoryIdCMSAsync(request.HistoryId, ct);
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

            await _audit.LogAsync(request.UserId, request.Username, "UpdatedHistory", $"Shrine #{shrineId} (History #{request.HistoryId})", true, null, ct);
        }
        catch (Exception e)
        {
            var target = shrineId != 0
                ? $"Shrine #{shrineId} (History #{request.HistoryId})"
                : $"Shrine -- (History #{request.HistoryId})";

            try
            {
                await _audit.LogAsync(request.UserId, request.Username, "UpdatedHistory", target, false, e.Message, ct);
            }
            catch { }
            throw;
        }

        return Unit.Value;
    }
}