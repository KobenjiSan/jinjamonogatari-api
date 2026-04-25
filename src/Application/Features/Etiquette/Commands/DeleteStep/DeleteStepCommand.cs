using MediatR;

namespace Application.Features.Etiquette.Commands.DeleteStep;

// COMMAND
public record DeleteStepCommand(int StepId) : IRequest<Unit>;