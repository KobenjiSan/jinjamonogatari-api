using MediatR;

namespace Application.Features.Shrines.Commands.CreateShrine;

// COMMAND
public record CreateShrineCommand(
    int UserId,
    string Username, 
    CreateShrineRequest Request
) : IRequest<Unit>;