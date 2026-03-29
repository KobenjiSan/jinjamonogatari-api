using MediatR;

namespace Application.Features.Shrines.Commands.UnlinkKamiToShrine;

// COMMAND
public record UnlinkKamiToShrineCommand(
    string UserRole,
    int ShrineId,
    int KamiId
) : IRequest<Unit>;