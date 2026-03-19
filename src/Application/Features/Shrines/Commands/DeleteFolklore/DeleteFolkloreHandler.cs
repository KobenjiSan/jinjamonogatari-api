using Application.Features.Shrines.Services;
using MediatR;

namespace Application.Features.Shrines.Commands.DeleteFolklore;

public class DeleteFolkloreHandler : IRequestHandler<DeleteFolkloreCommand, Unit>
{
    private readonly IShrineWriteService _shrineWriteService;

    public DeleteFolkloreHandler(IShrineWriteService shrineWriteService)
    {
        _shrineWriteService = shrineWriteService;
    }

    public async Task<Unit> Handle(DeleteFolkloreCommand request, CancellationToken ct)
    {
        await _shrineWriteService.DeleteFolkloreAsync(request.FolkloreId, ct);

        return Unit.Value;
    }
}