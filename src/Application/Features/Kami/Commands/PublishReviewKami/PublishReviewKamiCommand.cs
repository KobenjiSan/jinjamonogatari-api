using MediatR;

namespace Application.Features.Kami.Commands.PublishReviewKami;

// COMMAND
public record PublishReviewKamiCommand(
    string Username,
    int KamiId, 
    int UserId
) : IRequest<Unit>;