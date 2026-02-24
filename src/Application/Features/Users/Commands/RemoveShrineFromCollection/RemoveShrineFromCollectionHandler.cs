using Application.Features.Users.Services;
using MediatR;

namespace Application.Features.Collection.Commands.RemoveShrineFromCollection;

public class RemoveShrineFromCollectionHandler : IRequestHandler<RemoveShrineFromCollectionCommand>
{
    private readonly IUserWriteService _writeService;

    public RemoveShrineFromCollectionHandler(IUserWriteService writeService)
    {
        _writeService = writeService;
    }

    public async Task<Unit> Handle(RemoveShrineFromCollectionCommand request, CancellationToken ct)
    {
        await _writeService.RemoveShrineFromCollectionAsync(request.UserId, request.ShrineId, ct);
        return Unit.Value;
    }
}