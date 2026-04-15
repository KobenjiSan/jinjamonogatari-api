using Application.Features.Shrines.Services;
using Application.Features.Tags.Services;
using MediatR;

namespace Application.Features.Tags.Queries.GetAllTagsListCMS;

public class GetAllTagsListCMSHandler : IRequestHandler<GetAllTagsListCMSQuery, GetAllTagsListCMSResult>
{
    private readonly ITagsService _service;

    public GetAllTagsListCMSHandler(ITagsService service)
    {
        _service = service;
    }

    public async Task<GetAllTagsListCMSResult> Handle(GetAllTagsListCMSQuery request, CancellationToken ct)
    {
        var (tags, total) = await _service.GetAllTagsListCMSAsync(request, ct);
        return new GetAllTagsListCMSResult(tags, total);
    }
}