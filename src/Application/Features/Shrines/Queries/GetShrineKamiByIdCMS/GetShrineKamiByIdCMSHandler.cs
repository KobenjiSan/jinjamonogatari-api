using Application.Common.Models.Citations;
using Application.Common.Models.Images;
using Application.Features.Shrines.Models;
using Application.Features.Shrines.Services;
using Application.Features.Shrines.Services.ShrineAudit;
using MediatR;

namespace Application.Features.Shrines.Queries.GetShrineKamiByIdCMS;

public class GetShrineKamiByIdCMSHandler : IRequestHandler<GetShrineKamiByIdCMSQuery, GetShrineKamiByIdCMSResult>
{
    private readonly IShrineReadService _readService;
    private readonly IShrineAuditService _auditService;

    public GetShrineKamiByIdCMSHandler(IShrineReadService readService, IShrineAuditService auditService)
    {
        _readService = readService;
        _auditService = auditService;
    }

    public async Task<GetShrineKamiByIdCMSResult> Handle(GetShrineKamiByIdCMSQuery request, CancellationToken ct)
    {
        var kami = await _readService.GetShrineKamiByIdCMSAsync(request.Id, ct);

        var auditedKami = kami
            .Select(k => k with
            {
                Audit = _auditService.EvaluateKami(MapKamiSnapshot(k))
            })
            .ToList();

        return new GetShrineKamiByIdCMSResult(auditedKami);
    }
    
    private static KamiAuditSnapshot MapKamiSnapshot(KamiReadCMSDto kami)
    {
        return new KamiAuditSnapshot
        {
            KamiId = kami.KamiId,
            NameEn = kami.NameEn,
            NameJp = kami.NameJp,
            Desc = kami.Desc,
            Image = kami.Image is null ? null : MapImageSnapshot(kami.Image),
            Citations = kami.Citations
                .Select(MapCitationSnapshot)
                .ToList()
        };
    }

    private static ImageAuditSnapshot MapImageSnapshot(ImageCMSDto image)
    {
        return new ImageAuditSnapshot
        {
            ImgId = image.ImgId,
            ImgSource = image.ImageUrl,
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