using MediatR;

namespace Application.Features.Shrines.Commands.LinkKamiToShrine;

// COMMAND
public record LinkKamiToShrineCommand(
    int ShrineId,
    int KamiId
) : IRequest<Unit>;