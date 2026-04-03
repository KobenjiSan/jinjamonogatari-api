using Application.Features.Shrines.Services;
using MediatR;

namespace Application.Features.Shrines.Queries.GetShrineCitationsDropdownByIdCMS;

public class GetShrineCitationsDropdownByIdCMSHandler 
    : IRequestHandler<GetShrineCitationsDropdownByIdCMSQuery, GetShrineCitationsDropdownByIdCMSResult>
{
    private readonly IShrineReadService _readService;

    public GetShrineCitationsDropdownByIdCMSHandler(IShrineReadService readService)
    {
        _readService = readService;
    }

    public async Task<GetShrineCitationsDropdownByIdCMSResult> Handle(
        GetShrineCitationsDropdownByIdCMSQuery request,
        CancellationToken ct)
    {
        var citations = await _readService.GetShrineCitationsDropdownByIdCMSAsync(request.ShrineId, ct);

        return new GetShrineCitationsDropdownByIdCMSResult(citations);
    }
}