using Application.Features.Shrines.Services;
using MediatR;

namespace Application.Features.Shrines.Queries.GetShrineStatusByIdCMS;

public class GetShrineStatusByIdCMSHandler : IRequestHandler<GetShrineStatusByIdCMSQuery, GetShrineStatusByIdCMSResult>
{
    private readonly IShrineReadService _readService;

    public GetShrineStatusByIdCMSHandler(IShrineReadService readService)
    {
        _readService = readService;
    }

    public async Task<GetShrineStatusByIdCMSResult> Handle(GetShrineStatusByIdCMSQuery request, CancellationToken ct)
    {
        var status = await _readService.GetShrineStatusByIdCMSAsync(request.Id, ct);
        return new GetShrineStatusByIdCMSResult(status);
    }
}