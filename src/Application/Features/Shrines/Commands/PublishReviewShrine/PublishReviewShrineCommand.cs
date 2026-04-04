using MediatR;

namespace Application.Features.Shrines.Commands.PublishReviewShrine;

// COMMAND
public record PublishReviewShrineCommand(int ShrineId, int UserId) : IRequest<Unit>;