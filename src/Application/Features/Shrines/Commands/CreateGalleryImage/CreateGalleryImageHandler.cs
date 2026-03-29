using Application.Common.Policies;
using Application.Features.Shrines.Services;
using MediatR;

namespace Application.Features.Shrines.Commands.CreateGalleryImage;

public class CreateGalleryImageHandler : IRequestHandler<CreateGalleryImageCommand, Unit>
{
    private readonly IShrineWriteService _shrineWriteService;
    private readonly IShrineReadService _shrineReadService;

    public CreateGalleryImageHandler(IShrineWriteService shrineWriteService, IShrineReadService shrineReadService)
    {
        _shrineWriteService = shrineWriteService;
        _shrineReadService = shrineReadService;
    }

    public async Task<Unit> Handle(CreateGalleryImageCommand request, CancellationToken ct)
    {
        // Validate Policy 
        var shrineStatus = await _shrineReadService.GetShrineStatusByIdCMSAsync(request.ShrineId, ct);
        ShrineWritePolicy.EnsureCanModify(shrineStatus, request.UserRole);

        // Create Gallery Image
        await _shrineWriteService.CreateGalleryImageAsync(
            request.ShrineId,
            request.Request,
            ct
        );

        return Unit.Value;
    }
}