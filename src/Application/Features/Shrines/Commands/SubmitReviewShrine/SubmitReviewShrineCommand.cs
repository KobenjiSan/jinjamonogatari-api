using MediatR;

namespace Application.Features.Shrines.Commands.SubmitReviewShrine;

// COMMAND
public record SubmitReviewShrineCommand(
    string Username,
    int ShrineId, 
    int UserId
) : IRequest<Unit>;