using MediatR;

namespace Application.Features.Shrines.Commands.DeleteHeroImage;

// COMMAND
public record DeleteHeroImageCommand(
    int UserId, 
    string Username,
    string UserRole,
    int ShrineId,
    int ImageId
) : IRequest<Unit>;