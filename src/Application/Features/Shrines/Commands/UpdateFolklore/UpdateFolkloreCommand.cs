using MediatR;

namespace Application.Features.Shrines.Commands.UpdateFolklore;

// COMMAND
public record UpdateFolkloreCommand(string UserRole, int FolkloreId, UpdateFolkloreRequest Request) : IRequest<Unit>;