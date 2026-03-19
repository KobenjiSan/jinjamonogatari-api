using Application.Features.Shrines.Services;
using MediatR;

namespace Application.Features.Shrines.Commands.UpdateFolklore;

public class UpdateFolkloreHandler : IRequestHandler<UpdateFolkloreCommand, Unit>
{
    private readonly IShrineWriteService _shrineWriteService;

    public UpdateFolkloreHandler(IShrineWriteService shrineWriteService)
    {
        _shrineWriteService = shrineWriteService;
    }

    public async Task<Unit> Handle(UpdateFolkloreCommand request, CancellationToken ct)
    {
        await _shrineWriteService.UpdateFolkloreAsync(
            request.FolkloreId,
            request.Request,
            ct
        );

        return Unit.Value;
    }
}