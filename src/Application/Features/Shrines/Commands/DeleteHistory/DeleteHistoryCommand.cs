using MediatR;

namespace Application.Features.Shrines.Commands.DeleteHistory;

// COMMAND
public record DeleteHistoryCommand(string UserRole, int HistoryId) : IRequest<Unit>;