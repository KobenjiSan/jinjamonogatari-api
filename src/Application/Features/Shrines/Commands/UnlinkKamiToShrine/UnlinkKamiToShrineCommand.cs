using MediatR;

namespace Application.Features.Shrines.Commands.UnlinkKamiToShrine;

// COMMAND
public record UnlinkKamiToShrineCommand(
    int UserId,
    string Username,
    string UserRole,
    int ShrineId,
    int KamiId
) : IRequest<Unit>;