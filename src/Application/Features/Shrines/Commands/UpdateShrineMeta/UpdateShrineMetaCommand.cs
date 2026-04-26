using MediatR;

namespace Application.Features.Shrines.Commands.UpdateShrineMeta;

// COMMAND
public record UpdateShrineMetaCommand(
    int UserId,
    string Username,
    string UserRole,
    int ShrineId,
    UpdateShrineMetaRequest Request
) : IRequest<Unit>;