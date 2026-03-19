using Application.Features.Shrines.Services;
using MediatR;

namespace Application.Features.Shrines.Commands.CreateFolklore;

public class CreateFolkloreHandler : IRequestHandler<CreateFolkloreCommand, Unit>
{
    private readonly IShrineWriteService _shrineWriteService;

    public CreateFolkloreHandler(IShrineWriteService shrineWriteService)
    {
        _shrineWriteService = shrineWriteService;
    }

    public async Task<Unit> Handle(CreateFolkloreCommand request, CancellationToken ct)
    {
        await _shrineWriteService.CreateFolkloreAsync(
            request.ShrineId,
            request.Request,
            ct
        );

        return Unit.Value;
    }
}