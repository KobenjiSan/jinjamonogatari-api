using Application.Common.Policies;
using Application.Features.Audits.Services;
using Application.Features.Images.Services;
using Application.Features.Shrines.Services;
using MediatR;

namespace Application.Features.Shrines.Commands.CreateHistory;

public class CreateHistoryHandler : IRequestHandler<CreateHistoryCommand, Unit>
{
    private readonly IShrineWriteService _shrineWriteService;
    private readonly IShrineReadService _shrineReadService;
    private readonly IImageService _imageService;
    private readonly IAuditService _audit;

    public CreateHistoryHandler(
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

    public async Task<Unit> Handle(CreateHistoryCommand request, CancellationToken ct)
    {
        try
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
                    var uploadResult = await _imageService.UploadAsync(file, $"jinja/shrines/{request.ShrineId}/history", ct);
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

            // Create History
            var historyId = await _shrineWriteService.CreateHistoryAsync(
                request.ShrineId,
                finalData,
                publicId,
                ct
            );

            await _audit.LogAsync(request.UserId, request.Username, "CreatedHistory", $"Shrine #{request.ShrineId} (History #{historyId})", true, null, ct);
        }
        catch (Exception e)
        {
            try
            {
                await _audit.LogAsync(request.UserId, request.Username, "CreatedHistory", $"Shrine #{request.ShrineId}", false, e.Message, ct);
            }
            catch { }
            throw;
        }

        return Unit.Value;
    }
}