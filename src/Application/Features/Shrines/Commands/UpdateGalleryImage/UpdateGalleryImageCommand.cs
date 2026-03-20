using MediatR;

namespace Application.Features.Shrines.Commands.UpdateGalleryImage;

// COMMAND
public record UpdateGalleryImageCommand(int ImageId, ImageRequest Request) : IRequest<Unit>;