using Application.Features.Tags.Services;
using MediatR;

namespace Application.Features.Tags.Commands.DeleteTag;

public class DeleteTagHandler : IRequestHandler<DeleteTagCommand, Unit>
{
    private readonly ITagsService _service;

    public DeleteTagHandler(ITagsService service)
    {
        _service = service;
    }

    public async Task<Unit> Handle(DeleteTagCommand request, CancellationToken ct)
    {
        await _service.DeleteTagAsync(request.TagId, ct);
        return Unit.Value;
    }
}