using MediatR;

namespace Application.Features.Kami.Commands.WithdrawDraftKami;

// COMMAND
public record WithdrawDraftKamiCommand(
    string Username,
    int KamiId, 
    int UserId
) : IRequest<Unit>;