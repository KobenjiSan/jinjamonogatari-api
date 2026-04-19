using MediatR;

namespace Application.Features.Kami.Commands.CreateKami;

// COMMAND
public record CreateKamiCommand(
    CreateKamiInShrineRequest Request,
    IFormFile? File
) : IRequest<Unit>;