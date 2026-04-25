using Api.Controllers.Etiquette;
using MediatR;

namespace Application.Features.Etiquette.Commands.CreateEtiquette;

// COMMAND
public record CreateEtiquetteCommand(CreateEtiquetteRequest Request) : IRequest<Unit>;