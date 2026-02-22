using Application.Features.Shrines.Services;
using MediatR;

namespace Application.Features.Shrines.Queries.GetShrineFolkloreBySlug;

public class GetShrineFolkloreBySlugHandler : IRequestHandler<GetShrineFolkloreBySlugQuery, GetShrineFolkloreBySlugResult>
{
    private readonly IShrineReadService _readService;

    public GetShrineFolkloreBySlugHandler(IShrineReadService readService)
    {
        _readService = readService;
    }

    public async Task<GetShrineFolkloreBySlugResult> Handle(GetShrineFolkloreBySlugQuery request, CancellationToken ct)
    {
        var folklore = await _readService.GetShrineFolkloreBySlugAsync(request.Slug, ct);
        return new GetShrineFolkloreBySlugResult(folklore);
    }
}