using MediatR;

namespace Application.Features.Shrines.Commands.UpdateHistory;

// COMMAND
public record UpdateHistoryCommand(int HistoryId, UpdateHistoryRequest Request) : IRequest<Unit>;