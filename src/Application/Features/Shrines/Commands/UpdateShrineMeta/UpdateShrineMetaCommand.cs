using MediatR;

namespace Application.Features.Shrines.Commands.UpdateShrineMeta;

// COMMAND
public record UpdateShrineMetaCommand(
    string UserRole,
    int ShrineId,
    UpdateShrineMetaRequest Request
) : IRequest<Unit>;