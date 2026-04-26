using MediatR;
using Application.Common.Models.Images;

namespace Application.Features.Shrines.Commands.UpdateHeroImage;

// COMMAND
public record UpdateHeroImageCommand(
    int UserId, 
    string Username,
    string UserRole,
    int ShrineId,
    UpdateImageFormRequest Request
) : IRequest<ImageFullDto>;