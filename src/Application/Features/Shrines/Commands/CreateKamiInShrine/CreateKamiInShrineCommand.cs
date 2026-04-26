using MediatR;

namespace Application.Features.Shrines.Commands.CreateKamiInShrine;

// COMMAND
public record CreateKamiInShrineCommand(
    int UserId,
    string Username,
    string UserRole,
    int ShrineId,
    CreateKamiInShrineRequest Request,
    IFormFile? File
) : IRequest<Unit>;