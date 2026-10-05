using Application.Common.Services;
using Application.Features.Audits.Services;
using Application.Features.Images.Services;
using Application.Features.Kami.Services;
using MediatR;

namespace Application.Features.Kami.Commands.CreateKami;

public class CreateKamiHandler : IRequestHandler<CreateKamiCommand, Unit>
{
    private readonly IKamiService _service;
    private readonly IImageService _imageService;
    private readonly IEntityAuditService _entityAuditService;
    private readonly IAuditService _audit;

    public CreateKamiHandler
    (
        IKamiService service,
        IImageService imageService,
        IEntityAuditService entityAuditService,
        IAuditService audit
    )
    {
        _service = service;
        _imageService = imageService;
        _entityAuditService = entityAuditService;
        _audit = audit;
    }

    public async Task<Unit> Handle(CreateKamiCommand request, CancellationToken ct)
    {
        try
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
            var kamiId = await _service.CreateKamiAsync(
                finalData,
                publicId,
                ct
            );

            // run EntityAudit
            await _entityAuditService.AuditKamiAsync(kamiId, ct);

            // Main Audit
            await _audit.LogAsync(request.UserId, request.Username, "CreatedKami", $"Kami Management (Kami #{kamiId})", true, null, ct);
        }
        catch (Exception e)
        {
            try
            {
                await _audit.LogAsync(request.UserId, request.Username, "CreatedKami", "Kami Management", false, e.Message, ct);
            }
            catch { }
            throw;
        }
        
        return Unit.Value;
    }
}