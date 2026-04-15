using MediatR;

namespace Application.Features.Tags.Commands.UpdateTag;

// COMMAND
public record UpdateTagCommand(int TagId, TagRequest Request) : IRequest<Unit>;