using MediatR;

namespace Application.Features.Shrines.Commands.LinkKamiToShrine;

// COMMAND
public record LinkKamiToShrineCommand(
    int UserId,
    string Username,
    string UserRole,
    int ShrineId,
    int KamiId
) : IRequest<Unit>;