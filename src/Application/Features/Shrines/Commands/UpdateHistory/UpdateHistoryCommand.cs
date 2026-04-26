using MediatR;

namespace Application.Features.Shrines.Commands.UpdateHistory;

// COMMAND
public record UpdateHistoryCommand(
    int UserId, 
    string Username,
    string UserRole, 
    int HistoryId, 
    UpdateHistoryRequest Request, 
    IFormFile? File
) : IRequest<Unit>;