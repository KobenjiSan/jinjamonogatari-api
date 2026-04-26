using Application.Common.Models.Images;
using Application.Common.Policies;
using Application.Features.Audits.Services;
using Application.Features.Shrines.Services;
using MediatR;

namespace Application.Features.Shrines.Commands.UpdateHeroImage;

public class UpdateHeroImageHandler : IRequestHandler<UpdateHeroImageCommand, ImageFullDto>
{
    private readonly IShrineWriteService _shrineWriteService;
    private readonly IShrineReadService _shrineReadService;
    private readonly IAuditService _audit;

    public UpdateHeroImageHandler(
        IShrineWriteService shrineWriteService,
        IShrineReadService shrineReadService,
        IAuditService audit
    )
    {
        _shrineWriteService = shrineWriteService;
        _shrineReadService = shrineReadService;
        _audit = audit;
    }

    public async Task<ImageFullDto> Handle(UpdateHeroImageCommand request, CancellationToken ct)
    {
        ImageFullDto result;
        try
        {
            // Validate Policy
            var shrineStatus = await _shrineReadService.GetShrineStatusByIdCMSAsync(request.ShrineId, ct);
            ShrineWritePolicy.EnsureCanModify(shrineStatus, request.UserRole);

            // Update Hero Image
            result = await _shrineWriteService.UpdateHeroImageAsync(
                request.ShrineId,
                request.Request,
                ct
            );

            await _audit.LogAsync(request.UserId, request.Username, "UpdatedHeroImage", $"Shrine #{request.ShrineId} (Hero Image #{request.Request.ImgId})", true, null, ct);
        }
        catch (Exception e)
        {
            try
            {
                await _audit.LogAsync(request.UserId, request.Username, "UpdatedHeroImage", $"Shrine #{request.ShrineId} (Hero Image #{request.Request.ImgId})", false, e.Message, ct);
            }
            catch { }
            throw;
        }

        return result;
    }
}