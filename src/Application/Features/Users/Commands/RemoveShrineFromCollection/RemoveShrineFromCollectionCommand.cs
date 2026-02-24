using MediatR;

namespace Application.Features.Collection.Commands.RemoveShrineFromCollection;

// COMMAND
public record RemoveShrineFromCollectionCommand(int UserId, int ShrineId) : IRequest;