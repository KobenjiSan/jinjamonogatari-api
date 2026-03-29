using Application.Common.Policies;
using Application.Features.Shrines.Services;
using MediatR;

namespace Application.Features.Shrines.Commands.CreateFolklore;

public class CreateFolkloreHandler : IRequestHandler<CreateFolkloreCommand, Unit>
{
    private readonly IShrineWriteService _shrineWriteService;
    private readonly IShrineReadService _shrineReadService;

    public CreateFolkloreHandler(IShrineWriteService shrineWriteService, IShrineReadService shrineReadService)
    {
        _shrineWriteService = shrineWriteService;
        _shrineReadService = shrineReadService;
    }

    public async Task<Unit> Handle(CreateFolkloreCommand request, CancellationToken ct)
    {
        // Validate Policy 
        var shrineStatus = await _shrineReadService.GetShrineStatusByIdCMSAsync(request.ShrineId, ct);
        ShrineWritePolicy.EnsureCanModify(shrineStatus, request.UserRole);

        // Create Folklore
        await _shrineWriteService.CreateFolkloreAsync(
            request.ShrineId,
            request.Request,
            ct
        );

        return Unit.Value;
    }
}