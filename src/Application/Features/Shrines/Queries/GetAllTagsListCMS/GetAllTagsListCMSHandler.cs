using Application.Features.Shrines.Services;
using MediatR;

namespace Application.Features.Shrines.Queries.GetAllTagsListCMS;

public class GetAllTagsListCMSHandler : IRequestHandler<GetAllTagsListCMSQuery, GetAllTagsListCMSResult>
{
    private readonly IShrineReadService _readService;

    public GetAllTagsListCMSHandler(IShrineReadService readService)
    {
        _readService = readService;
    }

    public async Task<GetAllTagsListCMSResult> Handle(GetAllTagsListCMSQuery request, CancellationToken ct)
    {
        var tags = await _readService.GetAllTagsListCMSAsync(ct);
        return new GetAllTagsListCMSResult(tags);
    }
}