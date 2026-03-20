using Application.Features.Shrines.Services;
using MediatR;

namespace Application.Features.Shrines.Commands.DeleteGalleryImage;

public class DeleteGalleryImageHandler : IRequestHandler<DeleteGalleryImageCommand, Unit>
{
    private readonly IShrineWriteService _shrineWriteService;

    public DeleteGalleryImageHandler(IShrineWriteService shrineWriteService)
    {
        _shrineWriteService = shrineWriteService;
    }

    public async Task<Unit> Handle(DeleteGalleryImageCommand request, CancellationToken ct)
    {
        await _shrineWriteService.DeleteGalleryImageAsync(request.ImageId, ct);

        return Unit.Value;
    }
}