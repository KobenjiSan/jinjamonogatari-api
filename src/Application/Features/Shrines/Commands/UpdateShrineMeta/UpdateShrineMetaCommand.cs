using MediatR;

namespace Application.Features.Shrines.Commands.UpdateShrineMeta;

// COMMAND
public record UpdateShrineMetaCommand(
    int ShrineId,
    UpdateShrineMetaRequest Request
) : IRequest<Unit>;