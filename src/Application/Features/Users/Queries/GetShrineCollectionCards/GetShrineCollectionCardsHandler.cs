using Application.Common.Exceptions;
using Application.Features.Shrines.Models;
using Application.Features.Users.Services;
using MediatR;

namespace Application.Features.Collection.Queries.GetShrineCollectionCards;

public class GetShrineCollectionCardsHandler : IRequestHandler<GetShrineCollectionCardsQuery, GetShrineCollectionCardsResult>
{
    private readonly IUserReadService _readService;

    public GetShrineCollectionCardsHandler(IUserReadService readService)
    {
        _readService = readService;
    }

    public async Task<GetShrineCollectionCardsResult> Handle(GetShrineCollectionCardsQuery request, CancellationToken ct)
    {
        IReadOnlyList<ShrinePreviewDto> cards =
            await _readService.GetShrineCollectionCards(request.UserId, request.Lat, request.Lon, request.Q, ct);
        return new GetShrineCollectionCardsResult(cards);
    }
}