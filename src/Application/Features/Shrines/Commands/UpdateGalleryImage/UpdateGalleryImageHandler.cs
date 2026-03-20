using Application.Features.Shrines.Services;
using MediatR;

namespace Application.Features.Shrines.Commands.UpdateGalleryImage;

public class UpdateGalleryImageHandler : IRequestHandler<UpdateGalleryImageCommand, Unit>
{
    private readonly IShrineWriteService _shrineWriteService;

    public UpdateGalleryImageHandler(IShrineWriteService shrineWriteService)
    {
        _shrineWriteService = shrineWriteService;
    }

    public async Task<Unit> Handle(UpdateGalleryImageCommand request, CancellationToken ct)
    {
        await _shrineWriteService.UpdateGalleryImageAsync(
            request.ImageId,
            request.Request,
            ct
        );

        return Unit.Value;
    }
}