using MediatR;

namespace Application.Features.Kami.Commands.UpdateKami;

// COMMAND
public record UpdateKamiCommand(int KamiId, UpdateKamiRequest Request, IFormFile? File) : IRequest<Unit>;