using MediatR;

namespace Application.Features.Shrines.Commands.RejectReviewShrine;

// COMMAND
public record RejectReviewShrineCommand(int ShrineId, int UserId, string Message) : IRequest<Unit>;