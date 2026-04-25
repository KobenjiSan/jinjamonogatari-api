using Api.Controllers.Etiquette;
using MediatR;

namespace Application.Features.Etiquette.Commands.UpdateStep;

// COMMAND
public record UpdateStepCommand(int StepId, UpdateStepRequest Request, IFormFile? File) : IRequest<Unit>;