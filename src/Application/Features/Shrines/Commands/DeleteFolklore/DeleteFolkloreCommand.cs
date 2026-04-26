using MediatR;

namespace Application.Features.Shrines.Commands.DeleteFolklore;

// COMMAND
public record DeleteFolkloreCommand(
    int UserId, 
    string Username, 
    string UserRole, 
    int FolkloreId
) : IRequest<Unit>;