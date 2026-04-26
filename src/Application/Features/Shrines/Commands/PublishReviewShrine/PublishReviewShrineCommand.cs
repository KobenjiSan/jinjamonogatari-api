using MediatR;

namespace Application.Features.Shrines.Commands.PublishReviewShrine;

// COMMAND
public record PublishReviewShrineCommand(
    string Username,
    int ShrineId, 
    int UserId
) : IRequest<Unit>;