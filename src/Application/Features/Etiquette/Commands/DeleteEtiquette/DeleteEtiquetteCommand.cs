using MediatR;

namespace Application.Features.Etiquette.Commands.DeleteEtiquette;

// COMMAND
public record DeleteEtiquetteCommand(int TopicId) : IRequest<Unit>;