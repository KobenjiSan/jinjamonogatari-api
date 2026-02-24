using MediatR;

namespace Application.Features.Collection.Queries.GetShrineCollectionIds;

// QUERY
public record GetShrineCollectionIdsQuery(int UserId) : IRequest<GetShrineCollectionIdsResult>;

// RESULT
public record GetShrineCollectionIdsResult(IReadOnlyList<int> ShrineIds);