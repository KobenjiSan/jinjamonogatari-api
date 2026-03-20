using Application.Features.Shrines.Services;
using MediatR;

namespace Application.Features.Shrines.Commands.CreateGalleryImage;

public class CreateGalleryImageHandler : IRequestHandler<CreateGalleryImageCommand, Unit>
{
    private readonly IShrineWriteService _shrineWriteService;

    public CreateGalleryImageHandler(IShrineWriteService shrineWriteService)
    {
        _shrineWriteService = shrineWriteService;
    }

    public async Task<Unit> Handle(CreateGalleryImageCommand request, CancellationToken ct)
    {
        await _shrineWriteService.CreateGalleryImageAsync(
            request.ShrineId,
            request.Request,
            ct
        );

        return Unit.Value;
    }
}