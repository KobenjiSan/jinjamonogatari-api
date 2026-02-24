using Application.Common.Exceptions;
using Application.Features.Users.Services;
using MediatR;

namespace Application.Features.Collection.Queries.GetShrineCollectionIds;

public class GetShrineCollectionIdsHandler : IRequestHandler<GetShrineCollectionIdsQuery, GetShrineCollectionIdsResult>
{
    private readonly IUserReadService _readService;

    public GetShrineCollectionIdsHandler(IUserReadService readService)
    {
        _readService = readService;
    }

    public async Task<GetShrineCollectionIdsResult> Handle(GetShrineCollectionIdsQuery request, CancellationToken ct)
    {
        IReadOnlyList<int> ids = await _readService.GetShrineCollectionIds(request.UserId, ct);
        return new GetShrineCollectionIdsResult(ids);
    }
}