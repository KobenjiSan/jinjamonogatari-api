using MediatR;

namespace Application.Features.Etiquette.Commands.DeleteGlance;

// COMMAND
public record DeleteGlanceCommand(int TopicId) : IRequest<Unit>;