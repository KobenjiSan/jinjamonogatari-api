using Application.Common.Models.Citations;
using Application.Common.Models.Images;
using Application.Features.Etiquette.Models;
using Application.Features.Etiquette.Queries.GetEtiquetteTopicDetail;
using Application.Features.Etiquette.Services;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.Etiquette;

public class EtiquetteReadService : IEtiquetteReadService
{
    private readonly AppDbContext _db;

    public EtiquetteReadService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IReadOnlyList<EtiquetteTopicSummaryDto>> GetTopicsAsync(CancellationToken ct)
    {
        return await _db.EtiquetteTopics
            .AsNoTracking()
            .OrderBy(t => t.GuideOrder ?? int.MaxValue)
            .ThenBy(t => t.TopicId)
            .Select(t => new EtiquetteTopicSummaryDto(
                t.TopicId,
                t.Slug,
                t.TitleLong,
                t.TitleShort,
                t.Summary,
                t.IconKey,
                t.IconSet,
                t.ImageId,
                t.ShowInGlance,
                t.ShowAsHighlight,
                t.GlanceOrder,
                t.GuideOrder,
                t.Status,
                t.PublishedAt,
                t.CreatedAt,
                t.UpdatedAt,
                t.Steps
                    .OrderBy(s => s.StepOrder ?? int.MaxValue)
                    .ThenBy(s => s.StepId)
                    .Select(s => new EtiquetteStepSummaryDto(
                        s.StepId,
                        s.StepOrder,
                        s.Text,
                        s.ImageId
                    ))
                    .ToList(),
                t.TopicCitations
                    .OrderBy(tc => tc.CreatedAt)
                    .ThenBy(tc => tc.CiteId)
                    .Select(tc => new CitationDto(
                        tc.Citation.CiteId,
                        tc.Citation.Title,
                        tc.Citation.Author,
                        tc.Citation.Url,
                        tc.Citation.Year,
                        tc.Citation.Notes
                    ))
                    .ToList()
            ))
            .ToListAsync(ct);
    }

    public Task<GetEtiquetteTopicDetailResult?> GetTopicDetailByIdAsync(int topicId, CancellationToken ct)
        => BuildTopicDetailQuery()
            .FirstOrDefaultAsync(x => x.Topic.TopicId == topicId, ct);

    public Task<GetEtiquetteTopicDetailResult?> GetTopicDetailBySlugAsync(string slug, CancellationToken ct)
        => BuildTopicDetailQuery()
            .FirstOrDefaultAsync(x => x.Topic.Slug == slug, ct);

    private IQueryable<GetEtiquetteTopicDetailResult> BuildTopicDetailQuery()
    {
        return _db.EtiquetteTopics
            .AsNoTracking()
            .Select(t => new GetEtiquetteTopicDetailResult(
                new EtiquetteTopicDetailDto(
                    t.TopicId,
                    t.Slug,
                    t.TitleLong,
                    t.TitleShort,
                    t.Summary,
                    t.IconKey,
                    t.IconSet,
                    t.ShowInGlance,
                    t.ShowAsHighlight,
                    t.GlanceOrder,
                    t.GuideOrder,
                    t.Status,
                    t.PublishedAt,
                    t.CreatedAt,
                    t.UpdatedAt,
                    t.Image == null
                        ? null
                        : new ImageDto(
                            t.Image.ImgId,
                            t.Image.ImgSource,
                            t.Image.Title,
                            t.Image.Desc,
                            t.Image.Citation == null
                                ? null
                                : new CitationDto(
                                    t.Image.Citation.CiteId,
                                    t.Image.Citation.Title,
                                    t.Image.Citation.Author,
                                    t.Image.Citation.Url,
                                    t.Image.Citation.Year,
                                    t.Image.Citation.Notes
                                )
                        ),
                    t.Steps
                        .OrderBy(s => s.StepOrder ?? int.MaxValue)
                        .ThenBy(s => s.StepId)
                        .Select(s => new EtiquetteStepDto(
                            s.StepId,
                            s.StepOrder,
                            s.Text,
                            s.Image == null
                                ? null
                                : new ImageDto(
                                    s.Image.ImgId,
                                    s.Image.ImgSource,
                                    s.Image.Title,
                                    s.Image.Desc,
                                    s.Image.Citation == null
                                        ? null
                                        : new CitationDto(
                                            s.Image.Citation.CiteId,
                                            s.Image.Citation.Title,
                                            s.Image.Citation.Author,
                                            s.Image.Citation.Url,
                                            s.Image.Citation.Year,
                                            s.Image.Citation.Notes
                                        )
                                )
                        ))
                        .ToList(),
                    t.TopicCitations
                        .OrderBy(tc => tc.CreatedAt)
                        .ThenBy(tc => tc.CiteId)
                        .Select(tc => new CitationDto(
                            tc.Citation.CiteId,
                            tc.Citation.Title,
                            tc.Citation.Author,
                            tc.Citation.Url,
                            tc.Citation.Year,
                            tc.Citation.Notes
                        ))
                        .ToList()
                )
            ));
    }
}
