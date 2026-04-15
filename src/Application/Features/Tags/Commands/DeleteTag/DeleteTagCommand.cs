using MediatR;

namespace Application.Features.Tags.Commands.DeleteTag;

// COMMAND
public record DeleteTagCommand(int TagId) : IRequest<Unit>;