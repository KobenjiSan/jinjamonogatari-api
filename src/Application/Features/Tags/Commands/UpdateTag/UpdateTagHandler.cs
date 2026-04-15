using Application.Features.Tags.Services;
using MediatR;

namespace Application.Features.Tags.Commands.UpdateTag;

public class UpdateTagHandler : IRequestHandler<UpdateTagCommand, Unit>
{
    private readonly ITagsService _service;

    public UpdateTagHandler(ITagsService service)
    {
        _service = service;
    }

    public async Task<Unit> Handle(UpdateTagCommand request, CancellationToken ct)
    {
        await _service.UpdateTagAsync(request.TagId, request.Request, ct);
        return Unit.Value;
    }
}