using MediatR;

namespace Application.Features.Shrines.Commands.CreateHistory;

// COMMAND
public record CreateHistoryCommand(
    string UserRole,
    int ShrineId,
    CreateHistoryRequest Request,
    IFormFile? File
) : IRequest<Unit>;