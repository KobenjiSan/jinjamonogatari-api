using Application.Features.Etiquette.Services;
using Application.Features.Images.Services;
using MediatR;

namespace Application.Features.Etiquette.Commands.DeleteStep;

public class DeleteStepHandler : IRequestHandler<DeleteStepCommand, Unit>
{
    private readonly IEtiquetteWriteService _writeService;
    private readonly IEtiquetteReadService _readService;
    private readonly IImageService _imageService;

    public DeleteStepHandler(
        IEtiquetteWriteService writeService, 
        IEtiquetteReadService readService,
        IImageService imageService
    )
    {
        _writeService = writeService;
        _readService = readService;
        _imageService = imageService;
    }

    public async Task<Unit> Handle(DeleteStepCommand request, CancellationToken ct)
    {
        // Remove Image from Cloudinary
        string? publicId = await _readService.GetStepImagePublicIdCMSAsync(request.StepId, ct);
        if (!string.IsNullOrWhiteSpace(publicId)) await _imageService.DeleteAsync(publicId, ct);

        await _writeService.DeleteStepAsync(request.StepId, ct);
        return Unit.Value;
    }
}