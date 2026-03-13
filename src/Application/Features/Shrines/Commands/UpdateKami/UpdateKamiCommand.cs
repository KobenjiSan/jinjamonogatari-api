using MediatR;

namespace Application.Features.Shrines.Commands.UpdateKami;

// COMMAND
public record UpdateKamiCommand(int KamiId, UpdateKamiRequest Request) : IRequest<Unit>;