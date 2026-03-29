using MediatR;

namespace Application.Features.Shrines.Commands.UpdateGalleryImage;

// COMMAND
public record UpdateGalleryImageCommand(string UserRole, int ImageId, ImageRequest Request) : IRequest<Unit>;