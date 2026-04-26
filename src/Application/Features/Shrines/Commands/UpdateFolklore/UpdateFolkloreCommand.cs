using MediatR;

namespace Application.Features.Shrines.Commands.UpdateFolklore;

// COMMAND
public record UpdateFolkloreCommand(
    int UserId,
    string Username,
    string UserRole, 
    int FolkloreId, 
    UpdateFolkloreRequest Request, 
    IFormFile? File
) : IRequest<Unit>;