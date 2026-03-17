using MediatR;

namespace Application.Features.Shrines.Commands.DeleteHistory;

// COMMAND
public record DeleteHistoryCommand(int HistoryId) : IRequest<Unit>;