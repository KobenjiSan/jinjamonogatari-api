using Api.Controllers.Etiquette;
using MediatR;

namespace Application.Features.Etiquette.Commands.CreateStep;

// COMMAND
public record CreateStepCommand(
    int TopicId,
    CreateStepRequest Request,
    IFormFile? File
) : IRequest<Unit>;