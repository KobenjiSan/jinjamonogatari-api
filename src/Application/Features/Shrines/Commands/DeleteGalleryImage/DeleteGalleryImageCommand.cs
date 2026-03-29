using MediatR;

namespace Application.Features.Shrines.Commands.DeleteGalleryImage;

// COMMAND
public record DeleteGalleryImageCommand(string UserRole, int ImageId) : IRequest<Unit>;