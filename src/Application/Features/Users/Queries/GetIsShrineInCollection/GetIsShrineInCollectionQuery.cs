using MediatR;

namespace Application.Features.Collection.Queries.GetIsShrineInCollection;

// QUERY
public record GetIsShrineInCollectionQuery(int UserId, int ShrineId) : IRequest<GetIsShrineInCollectionResult>;

// RESULT
public record GetIsShrineInCollectionResult(bool IsSaved);