using MediatR;

namespace Application.Features.Shrines.Commands.CreateFolklore;

// COMMAND
public record CreateFolkloreCommand(
    int ShrineId,
    CreateFolkloreRequest Request
) : IRequest<Unit>;