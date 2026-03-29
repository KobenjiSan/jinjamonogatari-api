using MediatR;

namespace Application.Features.Shrines.Commands.UpdateHistory;

// COMMAND
public record UpdateHistoryCommand(string UserRole, int HistoryId, UpdateHistoryRequest Request) : IRequest<Unit>;