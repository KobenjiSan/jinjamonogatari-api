using MediatR;

namespace Application.Features.Kami.Commands.DeleteKami;

// COMMAND
public record DeleteKamiCommand(int KamiId) : IRequest<Unit>;