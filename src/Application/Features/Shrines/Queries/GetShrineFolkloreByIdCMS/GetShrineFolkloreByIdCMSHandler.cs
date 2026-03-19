using Application.Features.Shrines.Services;
using MediatR;

namespace Application.Features.Shrines.Queries.GetShrineFolkloreByIdCMS;

public class GetShrineFolkloreByIdCMSHandler : IRequestHandler<GetShrineFolkloreByIdCMSQuery, GetShrineFolkloreByIdCMSResult>
{
    private readonly IShrineReadService _readService;

    public GetShrineFolkloreByIdCMSHandler(IShrineReadService readService)
    {
        _readService = readService;
    }

    public async Task<GetShrineFolkloreByIdCMSResult> Handle(GetShrineFolkloreByIdCMSQuery request, CancellationToken ct)
    {
        var folklore = await _readService.GetShrineFolkloreByIdCMSAsync(request.Id, ct);
        return new GetShrineFolkloreByIdCMSResult(folklore);
    }
}