using Application.Common.Models.Citations;
using Application.Common.Models.Images;
using Application.Features.Shrines.Models;
using Application.Features.Shrines.Services;
using Application.Features.Shrines.Services.ShrineAudit;
using MediatR;

namespace Application.Features.Shrines.Queries.GetShrineGalleryByIdCMS;

public class GetShrineGalleryByIdCMSHandler : IRequestHandler<GetShrineGalleryByIdCMSQuery, GetShrineGalleryByIdCMSResult>
{
    private readonly IShrineReadService _readService;
    private readonly IShrineAuditService _auditService;

    public GetShrineGalleryByIdCMSHandler(
        IShrineReadService readService,
        IShrineAuditService auditService)
    {
        _readService = readService;
        _auditService = auditService;
    }

    public async Task<GetShrineGalleryByIdCMSResult> Handle(GetShrineGalleryByIdCMSQuery request, CancellationToken ct)
    {
        var images = await _readService.GetShrineGalleryByIdCMSAsync(request.Id, ct);

        var auditedImages = images
            .Select(image => new ImageCMSAuditDto(
                image.ImgId,
                image.ImageUrl,
                image.Title,
                image.Desc,
                image.Citation,
                image.CreatedAt,
                image.UpdatedAt,
                _auditService.EvaluateGalleryImage(MapImageSnapshot(image))
            ))
            .ToList();

        return new GetShrineGalleryByIdCMSResult(auditedImages);
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