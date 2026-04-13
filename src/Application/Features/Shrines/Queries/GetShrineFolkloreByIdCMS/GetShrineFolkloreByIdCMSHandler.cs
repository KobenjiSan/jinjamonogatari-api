using Application.Common.Models.Citations;
using Application.Common.Models.Images;
using Application.Features.Shrines.Models;
using Application.Features.Shrines.Services;
using Application.Features.Shrines.Services.ShrineAudit;
using MediatR;

namespace Application.Features.Shrines.Queries.GetShrineFolkloreByIdCMS;

public class GetShrineFolkloreByIdCMSHandler : IRequestHandler<GetShrineFolkloreByIdCMSQuery, GetShrineFolkloreByIdCMSResult>
{
    private readonly IShrineReadService _readService;
    private readonly IShrineAuditService _auditService;

    public GetShrineFolkloreByIdCMSHandler(
        IShrineReadService readService,
        IShrineAuditService auditService)
    {
        _readService = readService;
        _auditService = auditService;
    }

    public async Task<GetShrineFolkloreByIdCMSResult> Handle(GetShrineFolkloreByIdCMSQuery request, CancellationToken ct)
    {
        var folklore = await _readService.GetShrineFolkloreByIdCMSAsync(request.Id, ct);

        var auditedFolklore = folklore
            .Select(f => f with
            {
                Audit = _auditService.EvaluateFolklore(MapFolkloreSnapshot(f))
            })
            .ToList();

        return new GetShrineFolkloreByIdCMSResult(auditedFolklore);
    }

    private static FolkloreAuditSnapshot MapFolkloreSnapshot(FolkloreReadCMSDto folklore)
    {
        return new FolkloreAuditSnapshot
        {
            FolkloreId = folklore.FolkloreId,
            SortOrder = folklore.SortOrder,
            Title = folklore.Title,
            Information = folklore.Information,
            Image = folklore.Image is null ? null : MapImageSnapshot(folklore.Image),
            Citations = folklore.Citations
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