using MediatR;

namespace Application.Features.Shrines.Commands.LinkKamiToShrine;

// COMMAND
public record LinkKamiToShrineCommand(
    string UserRole,
    int ShrineId,
    int KamiId
) : IRequest<Unit>;