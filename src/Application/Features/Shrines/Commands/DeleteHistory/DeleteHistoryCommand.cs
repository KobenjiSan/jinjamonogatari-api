using MediatR;

namespace Application.Features.Shrines.Commands.DeleteHistory;

// COMMAND
public record DeleteHistoryCommand(
    int UserId, 
    string Username, 
    string UserRole, 
    int HistoryId
) : IRequest<Unit>;