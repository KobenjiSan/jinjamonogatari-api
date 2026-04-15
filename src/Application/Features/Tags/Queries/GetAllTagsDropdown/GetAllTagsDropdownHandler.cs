using Application.Features.Shrines.Services;
using Application.Features.Tags.Services;
using MediatR;

namespace Application.Features.Tags.Queries.GetAllTagsDropdown;

public class GetAllTagsDropdownHandler : IRequestHandler<GetAllTagsDropdownQuery, GetAllTagsDropdownResult>
{
    private readonly ITagsService _service;

    public GetAllTagsDropdownHandler(ITagsService service)
    {
        _service = service;
    }

    public async Task<GetAllTagsDropdownResult> Handle(GetAllTagsDropdownQuery request, CancellationToken ct)
    {
        var tags = await _service.GetAllTagsDropdownAsync(ct);
        return new GetAllTagsDropdownResult(tags);
    }
}