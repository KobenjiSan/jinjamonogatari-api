using MediatR;

namespace Application.Features.Shrines.Commands.DeleteGalleryImage;

// COMMAND
public record DeleteGalleryImageCommand(int ImageId) : IRequest<Unit>;