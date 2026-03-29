using MediatR;

namespace Application.Features.Shrines.Commands.CreateKamiInShrine;

// COMMAND
public record CreateKamiInShrineCommand(
    string UserRole,
    int ShrineId,
    CreateKamiInShrineRequest Request
) : IRequest<Unit>;