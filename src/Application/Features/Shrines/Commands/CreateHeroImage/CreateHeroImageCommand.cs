using MediatR;
using Application.Common.Models.Images;

namespace Application.Features.Shrines.Commands.CreateHeroImage;

// COMMAND
public record CreateHeroImageCommand(
    string UserRole,
    int ShrineId,
    CreateImageFormRequest Request
) : IRequest<ImageFullDto>;