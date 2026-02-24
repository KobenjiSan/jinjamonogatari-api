using Application.Features.Users.Services;
using MediatR;

namespace Application.Features.Collection.Queries.GetIsShrineInCollection;

public class GetIsShrineInCollectionHandler : IRequestHandler<GetIsShrineInCollectionQuery, GetIsShrineInCollectionResult>
{
    private readonly IUserReadService _readService;

    public GetIsShrineInCollectionHandler(IUserReadService readService)
    {
        _readService = readService;
    }

    public async Task<GetIsShrineInCollectionResult> Handle(GetIsShrineInCollectionQuery request, CancellationToken ct)
    {
        var isSaved = await _readService.IsShrineInCollectionAsync(request.UserId, request.ShrineId, ct);
        return new GetIsShrineInCollectionResult(isSaved);
    }
}