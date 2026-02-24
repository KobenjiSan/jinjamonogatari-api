using MediatR;

namespace Application.Features.Collection.Commands.AddShrineToCollection;

// COMMAND
public record AddShrineToCollectionCommand(int UserId, int ShrineId) : IRequest;