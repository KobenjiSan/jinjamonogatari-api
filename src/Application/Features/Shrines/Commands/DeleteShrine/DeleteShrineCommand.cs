using MediatR;

namespace Application.Features.Shrines.Commands.DeleteShrine;

// COMMAND
public record DeleteShrineCommand(
    int UserId, 
    string Username, 
    int ShrineId
) : IRequest<Unit>;