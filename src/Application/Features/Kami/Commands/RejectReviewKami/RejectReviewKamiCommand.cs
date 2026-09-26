using MediatR;

namespace Application.Features.Kami.Commands.RejectReviewKami;

// COMMAND
public record RejectReviewKamiCommand(
    string Username,
    int KamiId, 
    int UserId, 
    string Message
) : IRequest<Unit>;