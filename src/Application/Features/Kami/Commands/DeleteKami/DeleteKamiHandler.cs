using Application.Features.Images.Services;
using Application.Features.Kami.Services;
using MediatR;

namespace Application.Features.Kami.Commands.DeleteKami;

public class DeleteKamiHandler : IRequestHandler<DeleteKamiCommand, Unit>
{
    private readonly IKamiService _service;
    private readonly IImageService _imageService;

    public DeleteKamiHandler(
        IKamiService service,
        IImageService imageService
    )
    {
        _service = service;
        _imageService = imageService;
    }

    public async Task<Unit> Handle(DeleteKamiCommand request, CancellationToken ct)
    {
        // Remove Image from Cloudinary
        string? publicId = await _service.GetKamiImagePublicIdCMSAsync(request.KamiId, ct);
        if (!string.IsNullOrWhiteSpace(publicId)) await _imageService.DeleteAsync(publicId, ct);
 
        await _service.DeleteKamiAsync(request.KamiId, ct);

        return Unit.Value;
    }
}