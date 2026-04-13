using Application.Common.Models.Citations;
using Application.Common.Models.Images;
using Application.Features.Etiquette.Models;
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

    public async Task<IReadOnlyList<EtiquetteTopicDto>> GetTopicsAsync(CancellationToken ct)
    {
        return await _db.EtiquetteTopics
            .AsNoTracking()
            .Where(t => t.PublishedAt != null)
            .OrderBy(t => t.GuideOrder ?? int.MaxValue)
            .ThenBy(t => t.TopicId)
            .Select(t => new EtiquetteTopicDto(
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
                t.Steps
                    .OrderBy(s => s.StepOrder ?? int.MaxValue)
                    .ThenBy(s => s.StepId)
                    .Select(s => new EtiquetteStepDto(
                        s.StepId,
                        s.StepOrder,
                        s.Text,
                        s.Image == null
                            ? null
                            : new ImageCitedDto(
                                s.Image.ImageUrl,
                                s.Image.Citation == null
                                    ? null
                                    : new CitationDto(
                                        s.Image.Citation.CiteId,
                                        s.Image.Citation.Title,
                                        s.Image.Citation.Author,
                                        s.Image.Citation.Url,
                                        s.Image.Citation.Year
                                    )
                            )
                    ))
                    .ToList(),
                t.TopicCitations
                    .OrderBy(tc => tc.CiteId)
                    .Select(tc => new CitationDto(
                        tc.Citation.CiteId,
                        tc.Citation.Title,
                        tc.Citation.Author,
                        tc.Citation.Url,
                        tc.Citation.Year
                    ))
                    .ToList()
            ))
            .ToListAsync(ct);
    }

    public async Task<EtiquetteTopicDto?> GetTopicDetailByIdAsync(int topicId, CancellationToken ct)
    {
        return await _db.EtiquetteTopics
            .AsNoTracking()
            .Where(t => t.TopicId == topicId && t.PublishedAt != null)
            .Select(t => new EtiquetteTopicDto(
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
                t.Steps
                    .OrderBy(s => s.StepOrder ?? int.MaxValue)
                    .ThenBy(s => s.StepId)
                    .Select(s => new EtiquetteStepDto(
                        s.StepId,
                        s.StepOrder,
                        s.Text,
                        s.Image == null
                            ? null
                            : new ImageCitedDto(
                                s.Image.ImageUrl,
                                s.Image.Citation == null
                                    ? null
                                    : new CitationDto(
                                        s.Image.Citation.CiteId,
                                        s.Image.Citation.Title,
                                        s.Image.Citation.Author,
                                        s.Image.Citation.Url,
                                        s.Image.Citation.CiteId == 0 ? null : s.Image.Citation.Year // harmless guard if you ever seed weird data
                                    )
                            )
                    ))
                    .ToList(),
                t.TopicCitations
                    .OrderBy(tc => tc.CiteId)
                    .Select(tc => new CitationDto(
                        tc.Citation.CiteId,
                        tc.Citation.Title,
                        tc.Citation.Author,
                        tc.Citation.Url,
                        tc.Citation.Year
                    ))
                    .ToList()
            ))
            .SingleOrDefaultAsync(ct);
    }

    public async Task<EtiquetteTopicDto?> GetTopicDetailBySlugAsync(string slug, CancellationToken ct)
    {
        return await _db.EtiquetteTopics
            .AsNoTracking()
            .Where(t => t.Slug == slug && t.PublishedAt != null)
            .Select(t => new EtiquetteTopicDto(
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
                t.Steps
                    .OrderBy(s => s.StepOrder ?? int.MaxValue)
                    .ThenBy(s => s.StepId)
                    .Select(s => new EtiquetteStepDto(
                        s.StepId,
                        s.StepOrder,
                        s.Text,
                        s.Image == null
                            ? null
                            : new ImageCitedDto(
                                s.Image.ImageUrl,
                                s.Image.Citation == null
                                    ? null
                                    : new CitationDto(
                                        s.Image.Citation.CiteId,
                                        s.Image.Citation.Title,
                                        s.Image.Citation.Author,
                                        s.Image.Citation.Url,
                                        s.Image.Citation.Year
                                    )
                            )
                    ))
                    .ToList(),
                t.TopicCitations
                    .OrderBy(tc => tc.CiteId)
                    .Select(tc => new CitationDto(
                        tc.Citation.CiteId,
                        tc.Citation.Title,
                        tc.Citation.Author,
                        tc.Citation.Url,
                        tc.Citation.Year
                    ))
                    .ToList()
            ))
            .SingleOrDefaultAsync(ct);
    }
}