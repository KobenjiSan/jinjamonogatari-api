using MediatR;

namespace Application.Features.Shrines.Commands.DeleteGalleryImage;

// COMMAND
public record DeleteGalleryImageCommand(
    int UserId, 
    string Username, 
    string UserRole, 
    int ImageId
) : IRequest<Unit>;