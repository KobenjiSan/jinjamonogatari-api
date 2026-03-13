using MediatR;

namespace Application.Features.Shrines.Commands.CreateKamiInShrine;

// COMMAND
public record CreateKamiInShrineCommand(
    int ShrineId,
    CreateKamiInShrineRequest Request
) : IRequest<Unit>;