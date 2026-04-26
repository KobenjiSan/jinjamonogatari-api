using MediatR;

namespace Application.Features.Shrines.Commands.RejectReviewShrine;

// COMMAND
public record RejectReviewShrineCommand(
    string Username,
    int ShrineId, 
    int UserId, 
    string Message
) : IRequest<Unit>;