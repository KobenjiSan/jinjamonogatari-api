using MediatR;

namespace Application.Features.Shrines.Commands.ImportShrines;

// COMMAND
public record ImportShrinesCommand(
    int UserId,
    string Username,
    ImportShrinesRequest Request
) : IRequest<Unit>;