using MediatR;

namespace Application.Features.Kami.Commands.DeleteKami;

// COMMAND
public record DeleteKamiCommand(
    int UserId,
    string Username,
    int KamiId
) : IRequest<Unit>;