using MediatR;

namespace Application.Features.Shrines.Commands.DeleteFolklore;

// COMMAND
public record DeleteFolkloreCommand(string UserRole, int FolkloreId) : IRequest<Unit>;