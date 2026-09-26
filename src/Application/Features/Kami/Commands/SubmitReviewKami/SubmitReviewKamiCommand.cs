using MediatR;

namespace Application.Features.Kami.Commands.SubmitReviewKami;

// COMMAND
public record SubmitReviewKamiCommand(
    string Username,
    int KamiId, 
    int UserId
) : IRequest<Unit>;
