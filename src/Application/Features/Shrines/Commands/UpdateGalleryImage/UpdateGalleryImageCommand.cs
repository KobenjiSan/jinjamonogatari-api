using MediatR;

namespace Application.Features.Shrines.Commands.UpdateGalleryImage;

// COMMAND
public record UpdateGalleryImageCommand(
    int UserId, 
    string Username, 
    string UserRole, 
    int ImageId,
    UpdateImageFormRequest Request
) : IRequest<Unit>;