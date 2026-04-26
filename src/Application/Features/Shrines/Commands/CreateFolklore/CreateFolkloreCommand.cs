using MediatR;

namespace Application.Features.Shrines.Commands.CreateFolklore;

// COMMAND
public record CreateFolkloreCommand(
    int UserId,
    string Username,
    string UserRole,
    int ShrineId,
    CreateFolkloreRequest Request,
    IFormFile? File
) : IRequest<Unit>;