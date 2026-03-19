using MediatR;

namespace Application.Features.Shrines.Commands.DeleteFolklore;

// COMMAND
public record DeleteFolkloreCommand(int FolkloreId) : IRequest<Unit>;