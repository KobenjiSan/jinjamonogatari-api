using Application.Features.Audits.Services;
using Application.Features.Images.Services;
using Application.Features.Kami.Services;
using MediatR;

namespace Application.Features.Kami.Commands.DeleteKami;

public class DeleteKamiHandler : IRequestHandler<DeleteKamiCommand, Unit>
{
    private readonly IKamiService _service;
    private readonly IImageService _imageService;
    private readonly IAuditService _audit;

    public DeleteKamiHandler(
        IKamiService service,
        IImageService imageService,
        IAuditService audit
    )
    {
        _service = service;
        _imageService = imageService;
        _audit = audit;
    }

    public async Task<Unit> Handle(DeleteKamiCommand request, CancellationToken ct)
    {
        try
        {
            // Remove Image from Cloudinary
            string? publicId = await _service.GetKamiImagePublicIdCMSAsync(request.KamiId, ct);
            if (!string.IsNullOrWhiteSpace(publicId)) await _imageService.DeleteAsync(publicId, ct);

            await _service.DeleteKamiAsync(request.KamiId, ct);

            // Main Audit
            await _audit.LogAsync(request.UserId, request.Username, "DeletedKami", $"Kami Management (Kami #{request.KamiId})", true, null, ct);
        }
        catch (Exception e)
        {
            try
            {
                await _audit.LogAsync(request.UserId, request.Username, "DeletedKami", $"Kami Management (Kami #{request.KamiId})", false, e.Message, ct);
            }
            catch { }
            throw;
        }

        return Unit.Value;
    }
}