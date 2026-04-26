using MediatR;

namespace Application.Features.Shrines.Commands.CreateHistory;

// COMMAND
public record CreateHistoryCommand(
    int UserId,
    string Username,
    string UserRole,
    int ShrineId,
    CreateHistoryRequest Request,
    IFormFile? File
) : IRequest<Unit>;