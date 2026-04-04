using MediatR;

namespace Application.Features.Shrines.Commands.SubmitReviewShrine;

// COMMAND
public record SubmitReviewShrineCommand(int ShrineId, int UserId) : IRequest<Unit>;