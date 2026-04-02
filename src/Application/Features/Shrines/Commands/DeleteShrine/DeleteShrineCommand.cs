using MediatR;

namespace Application.Features.Shrines.Commands.DeleteShrine;

// COMMAND
public record DeleteShrineCommand(int ShrineId) : IRequest<Unit>;