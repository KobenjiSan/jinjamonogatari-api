using MediatR;

namespace Application.Features.Shrines.Commands.CreateGalleryImage;

// COMMAND
public record CreateGalleryImageCommand(
    int UserId,
    string Username,
    string UserRole,
    int ShrineId,
    CreateImageFormRequest Request
) : IRequest<Unit>;