using Application.Common.Models.Images;
using Application.Common.Policies;
using Application.Features.Images.Services;
using Application.Features.Shrines.Services;
using MediatR;

namespace Application.Features.Shrines.Commands.UpdateHeroImage;

public class UpdateHeroImageHandler : IRequestHandler<UpdateHeroImageCommand, ImageFullDto>
{
    private readonly IShrineWriteService _shrineWriteService;
    private readonly IShrineReadService _shrineReadService;
    private readonly IImageService _imageService;

    public UpdateHeroImageHandler(
        IShrineWriteService shrineWriteService,
        IShrineReadService shrineReadService,
        IImageService imageService
    )
    {
        _shrineWriteService = shrineWriteService;
        _shrineReadService = shrineReadService;
        _imageService = imageService;
    }

    public async Task<ImageFullDto> Handle(UpdateHeroImageCommand request, CancellationToken ct)
    {
        // Validate Policy
        var shrineStatus = await _shrineReadService.GetShrineStatusByIdCMSAsync(request.ShrineId, ct);
        ShrineWritePolicy.EnsureCanModify(shrineStatus, request.UserRole);

        // Update Hero Image
        var result = await _shrineWriteService.UpdateHeroImageAsync(
            request.ShrineId,
            request.Request,
            ct
        );

        return result;
    }
}