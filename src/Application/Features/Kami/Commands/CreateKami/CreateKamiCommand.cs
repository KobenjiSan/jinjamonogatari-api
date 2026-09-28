using MediatR;

namespace Application.Features.Kami.Commands.CreateKami;

// COMMAND
public record CreateKamiCommand(
    int UserId,
    string Username,
    CreateKamiInShrineRequest Request,
    IFormFile? File
) : IRequest<Unit>;