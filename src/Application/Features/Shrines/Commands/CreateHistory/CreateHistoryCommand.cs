using MediatR;

namespace Application.Features.Shrines.Commands.CreateHistory;

// COMMAND
public record CreateHistoryCommand(
    int ShrineId,
    CreateHistoryRequest Request
) : IRequest<Unit>;