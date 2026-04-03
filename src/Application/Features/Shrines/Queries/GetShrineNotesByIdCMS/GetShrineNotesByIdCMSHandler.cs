using Application.Common.Exceptions;
using Application.Features.Shrines.Services;
using MediatR;

namespace Application.Features.Shrines.Queries.GetShrineNotesByIdCMS;

public class GetShrineNotesByIdCMSHandler : IRequestHandler<GetShrineNotesByIdCMSQuery, GetShrineNotesByIdCMSResult>
{
    private readonly IShrineReadService _readService;

    public GetShrineNotesByIdCMSHandler(IShrineReadService readService)
    {
        _readService = readService;
    }

    public async Task<GetShrineNotesByIdCMSResult> Handle(GetShrineNotesByIdCMSQuery request, CancellationToken ct)
    {
        var shrineNotes = await _readService.GetShrineNotesByIdCMSAsync(request.Id, ct);
        if (shrineNotes is null) throw new NotFoundException("Shrine not found.");
        return new GetShrineNotesByIdCMSResult(shrineNotes);
    }
}