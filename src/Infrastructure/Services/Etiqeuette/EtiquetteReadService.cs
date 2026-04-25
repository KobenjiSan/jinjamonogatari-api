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
                                        s.Image.Citation.CiteId == 0 ? null : s.Image.Citation.Year
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

    #region Get Glance Topics

    public async Task<IReadOnlyList<AtAGlanceCMSDto>> GetGlanceTopicsCMSAsync(CancellationToken ct)
    {
        return await _db.EtiquetteTopics
            .AsNoTracking()
            .Where(t => t.ShowInGlance == true)
            .OrderBy(t => t.GlanceOrder ?? int.MaxValue)
            .ThenBy(t => t.TopicId)
            .Select(t => new AtAGlanceCMSDto(
                t.TopicId,
                t.TitleLong,
                t.TitleShort,
                t.IconKey,
                t.IconSet,
                t.GlanceOrder
            ))
            .ToListAsync(ct);
    }

    #endregion

    #region Get Topics

    public async Task<IReadOnlyList<EtiquetteTopicCMSDto>> GetTopicsCMSAsync(CancellationToken ct)
    {
        return await _db.EtiquetteTopics
            .AsNoTracking()
            .OrderBy(t => t.TopicId)
            .Select(t => new EtiquetteTopicCMSDto(
                t.TopicId,
                t.Slug,
                t.TitleLong,
                t.Summary,
                t.ShowInGlance,
                t.ShowAsHighlight,
                t.GuideOrder,
                t.Status,
                t.CreatedAt,
                t.UpdatedAt,
                t.TopicCitations
                    .OrderBy(tc => tc.CiteId)
                    .Select(tc => new CitationCMSDto(
                        tc.Citation.CiteId,
                        tc.Citation.Title,
                        tc.Citation.Author,
                        tc.Citation.Url,
                        tc.Citation.Year,
                        tc.Citation.CreatedAt,
                        tc.Citation.UpdatedAt
                    )).ToList()
            )).ToListAsync(ct);
    }

    #endregion

    #region Get Steps By Id

    public async Task<IReadOnlyList<EtiquetteStepCMSDto>> GetStepsByIdCMSAsync(int topicId, CancellationToken ct)
    {
        return await _db.EtiquetteSteps
            .AsNoTracking()
            .Where(s => s.TopicId == topicId)
            .OrderBy(s => s.StepOrder ?? int.MaxValue)
            .ThenBy(s => s.StepId)
            .Select(s => new EtiquetteStepCMSDto(
                s.StepId,
                s.Text,
                s.StepOrder,
                s.Image == null
                    ? null
                    : new ImageCMSDto(
                        s.Image.ImgId,
                        s.Image.ImageUrl,
                        s.Image.Title,
                        s.Image.Desc,
                        s.Image.Citation == null
                            ? null
                            : new CitationCMSDto(
                                s.Image.Citation.CiteId,
                                s.Image.Citation.Title,
                                s.Image.Citation.Author,
                                s.Image.Citation.Url,
                                s.Image.Citation.Year,
                                s.Image.Citation.CreatedAt,
                                s.Image.Citation.UpdatedAt
                            ),
                        s.Image.CreatedAt,
                        s.Image.UpdatedAt
                    )
            )).ToListAsync(ct);
    }

    #endregion

    #region Get PublicId Step

    public async Task<string?> GetStepImagePublicIdCMSAsync(int stepId, CancellationToken ct)
    {
        return await _db.EtiquetteSteps
            .AsNoTracking()
            .Where(s => s.StepId == stepId)
            .Select(s => s.Image!.PublicId)
            .SingleOrDefaultAsync(ct);
    }

    #endregion
}