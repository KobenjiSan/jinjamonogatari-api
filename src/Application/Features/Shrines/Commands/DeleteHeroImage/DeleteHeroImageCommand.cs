using MediatR;

namespace Application.Features.Shrines.Commands.DeleteHeroImage;

// COMMAND
public record DeleteHeroImageCommand(
    string UserRole,
    int ShrineId,
    int ImageId
) : IRequest<Unit>;