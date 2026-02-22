using Application.Common.Exceptions;
using Application.Features.Shrines.Services;
using MediatR;

namespace Application.Features.Shrines.Queries.GetImageById;

public class GetImageByIdHandler : IRequestHandler<GetImageByIdQuery, GetImageByIdResult>
{
    private readonly IShrineReadService _readService;

    public GetImageByIdHandler(IShrineReadService readService)
    {
        _readService = readService;
    }

    public async Task<GetImageByIdResult> Handle(GetImageByIdQuery request, CancellationToken ct)
    {
        var image = await _readService.GetImageByIdAsync(request.Id, ct);
        if (image is null) throw new NotFoundException("Image not found.");
        return new GetImageByIdResult(image);
    }
}