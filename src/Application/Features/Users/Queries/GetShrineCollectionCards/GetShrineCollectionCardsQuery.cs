using Application.Features.Shrines.Models;
using MediatR;

namespace Application.Features.Collection.Queries.GetShrineCollectionCards;

// QUERY
public record GetShrineCollectionCardsQuery(int UserId) : IRequest<GetShrineCollectionCardsResult>;

// RESULT
public record GetShrineCollectionCardsResult(IReadOnlyList<ShrinePreviewDto> Cards);