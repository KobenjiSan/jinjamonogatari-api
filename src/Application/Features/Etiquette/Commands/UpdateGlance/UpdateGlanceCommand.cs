using Api.Controllers.Etiquette;
using MediatR;

namespace Application.Features.Etiquette.Commands.UpdateGlance;

// COMMAND
public record UpdateGlanceCommand(int TopicId, UpdateGlanceRequest Request) : IRequest<Unit>;