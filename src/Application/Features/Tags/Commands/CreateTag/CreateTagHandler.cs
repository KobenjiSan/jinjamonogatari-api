using Application.Features.Tags.Services;
using MediatR;

namespace Application.Features.Tags.Commands.CreateTag;

public class CreateTagHandler : IRequestHandler<CreateTagCommand, Unit>
{
    private readonly ITagsService _service;

    public CreateTagHandler(ITagsService service)
    {
        _service = service;
    }

    public async Task<Unit> Handle(CreateTagCommand request, CancellationToken ct)
    {
        await _service.CreateTagAsync(request.Request, ct);
        return Unit.Value;
    }
}