using Application.Common.Models.Citations;
using Application.Common.Models.Images;
using Application.Features.Shrines.Models;
using Application.Features.Shrines.Services;
using Application.Features.Shrines.Services.ShrineAudit;
using MediatR;

namespace Application.Features.Shrines.Queries.GetShrineHistoryByIdCMS;

public class GetShrineHistoryByIdCMSHandler : IRequestHandler<GetShrineHistoryByIdCMSQuery, GetShrineHistoryByIdCMSResult>
{
    private readonly IShrineReadService _readService;
    private readonly IShrineAuditService _auditService;

    public GetShrineHistoryByIdCMSHandler(
        IShrineReadService readService,
        IShrineAuditService auditService)
    {
        _readService = readService;
        _auditService = auditService;
    }

    public async Task<GetShrineHistoryByIdCMSResult> Handle(GetShrineHistoryByIdCMSQuery request, CancellationToken ct)
    {
        var history = await _readService.GetShrineHistoryByIdCMSAsync(request.Id, ct);

        var auditedHistory = history
            .Select(h => h with
            {
                Audit = _auditService.EvaluateHistory(MapHistorySnapshot(h))
            })
            .ToList();

        return new GetShrineHistoryByIdCMSResult(auditedHistory);
    }

    private static HistoryAuditSnapshot MapHistorySnapshot(HistoryReadCMSDto history)
    {
        return new HistoryAuditSnapshot
        {
            HistoryId = history.HistoryId,
            EventDate = history.EventDate,
            SortOrder = history.SortOrder,
            Title = history.Title,
            Information = history.Information,
            Image = history.Image is null ? null : MapImageSnapshot(history.Image),
            Citations = history.Citations
                .Select(MapCitationSnapshot)
                .ToList()
        };
    }

    private static ImageAuditSnapshot MapImageSnapshot(ImageCMSDto image)
    {
        return new ImageAuditSnapshot
        {
            ImgId = image.ImgId,
            ImageUrl = image.ImageUrl,
            Title = image.Title,
            Desc = image.Desc,
            Citation = image.Citation is null ? null : MapCitationSnapshot(image.Citation)
        };
    }

    private static CitationAuditSnapshot MapCitationSnapshot(CitationCMSDto citation)
    {
        return new CitationAuditSnapshot
        {
            CiteId = citation.CiteId,
            Title = citation.Title,
            Author = citation.Author,
            Url = citation.Url,
            Year = citation.Year
        };
    }
}