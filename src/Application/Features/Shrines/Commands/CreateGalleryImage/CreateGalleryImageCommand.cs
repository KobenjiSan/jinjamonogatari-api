using MediatR;

namespace Application.Features.Shrines.Commands.CreateGalleryImage;

// COMMAND
public record CreateGalleryImageCommand(
    int ShrineId,
    CreateImageRequest Request
) : IRequest<Unit>;