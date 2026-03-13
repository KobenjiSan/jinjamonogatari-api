using MediatR;

namespace Application.Features.Shrines.Commands.UnlinkKamiToShrine;

// COMMAND
public record UnlinkKamiToShrineCommand(
    int ShrineId,
    int KamiId
) : IRequest<Unit>;