using Application.Features.Shrines.Services;
using MediatR;

namespace Application.Features.Shrines.Queries.GetShrineCitationsByIdCMS;

public class GetShrineCitationsByIdCMSHandler 
    : IRequestHandler<GetShrineCitationsByIdCMSQuery, GetShrineCitationsByIdCMSResult>
{
    private readonly IShrineReadService _readService;

    public GetShrineCitationsByIdCMSHandler(IShrineReadService readService)
    {
        _readService = readService;
    }

    public async Task<GetShrineCitationsByIdCMSResult> Handle(
        GetShrineCitationsByIdCMSQuery request,
        CancellationToken ct)
    {
        var citations = await _readService.GetShrineCitationsByIdCMSAsync(request.ShrineId, ct);

        return new GetShrineCitationsByIdCMSResult(citations);
    }
}