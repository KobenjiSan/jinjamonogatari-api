using MediatR;

namespace Application.Features.Shrines.Commands.CreateShrine;

// COMMAND
public record CreateShrineCommand(CreateShrineRequest Request) : IRequest<Unit>;