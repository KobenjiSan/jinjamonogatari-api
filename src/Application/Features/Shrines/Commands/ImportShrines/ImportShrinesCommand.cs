using MediatR;

namespace Application.Features.Shrines.Commands.ImportShrines;

// COMMAND
public record ImportShrinesCommand(ImportShrinesRequest Request) : IRequest<Unit>;