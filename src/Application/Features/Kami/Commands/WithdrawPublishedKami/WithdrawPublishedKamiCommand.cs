using MediatR;

namespace Application.Features.Kami.Commands.WithdrawPublishedKami;

// COMMAND
public record WithdrawPublishedKamiCommand(
    string Username,
    int KamiId, 
    int UserId, 
    string Message
) : IRequest<Unit>;