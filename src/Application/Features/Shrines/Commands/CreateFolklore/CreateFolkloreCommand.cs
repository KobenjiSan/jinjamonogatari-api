using MediatR;

namespace Application.Features.Shrines.Commands.CreateFolklore;

// COMMAND
public record CreateFolkloreCommand(
    string UserRole,
    int ShrineId,
    CreateFolkloreRequest Request,
    IFormFile? File
) : IRequest<Unit>;