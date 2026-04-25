using Api.Controllers.Etiquette;
using MediatR;

namespace Application.Features.Etiquette.Commands.UpdateEtiquette;

// COMMAND
public record UpdateEtiquetteCommand(int TopicId, UpdateEtiquetteRequest Request) : IRequest<Unit>;