using Application.Common.Exceptions;
using Application.Features.Shrines.Services;
using Application.Features.Users.Services;
using MediatR;

namespace Application.Features.Collection.Commands.AddShrineToCollection;

public class AddShrineToCollectionHandler : IRequestHandler<AddShrineToCollectionCommand>
{
    private readonly IUserWriteService _writeService;
    private readonly IShrineReadService _shrineReadService;

    public AddShrineToCollectionHandler(IUserWriteService writeService, IShrineReadService shrineReadService)
    {
        _writeService = writeService;
        _shrineReadService = shrineReadService;
    }

    public async Task<Unit> Handle(AddShrineToCollectionCommand request, CancellationToken ct)
    {
        if (!await _shrineReadService.DoesShrineExistAsync(request.ShrineId, ct))
            throw new NotFoundException("Shrine not found.");

        await _writeService.AddShrineToCollectionAsync(request.UserId, request.ShrineId, ct);
        return Unit.Value;
    }
}