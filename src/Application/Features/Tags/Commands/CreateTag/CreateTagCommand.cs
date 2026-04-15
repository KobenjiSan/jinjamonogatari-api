using MediatR;

namespace Application.Features.Tags.Commands.CreateTag;

// COMMAND
public record CreateTagCommand(TagRequest Request) : IRequest<Unit>;