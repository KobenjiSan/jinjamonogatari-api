using MediatR;

namespace Application.Features.Shrines.Commands.CreateGalleryImage;

// COMMAND
public record CreateGalleryImageCommand(
    string UserRole,
    int ShrineId,
    CreateGalleryImageFormRequest Request
) : IRequest<Unit>;