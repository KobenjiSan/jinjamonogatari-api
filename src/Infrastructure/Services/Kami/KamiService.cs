using Application.Common.Exceptions;
using Application.Common.Models.Citations;
using Application.Common.Models.Images;
using Application.Features.Kami.Queries.GetAllKamiCMS;
using Application.Features.Kami.Services;
using Application.Features.Shrines.Models;
using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.KamiService;

public class KamiService : IKamiService
{
    private readonly AppDbContext _db;

    public KamiService(AppDbContext db)
    {
        _db = db;
    }

    #region Citation Helper

    private static Citation BuildCitation(CreateCitationRequest request)
    {
        return new Citation
        {
            Title = request.Title,
            Author = request.Author,
            Url = request.Url,
            Year = request.Year
        };
    }

    private async Task<Citation> GetExistingCitationOrThrowAsync(int citeId, CancellationToken ct)
    {
        var citation = await _db.Citations
            .FirstOrDefaultAsync(c => c.CiteId == citeId, ct);

        if (citation is null)
            throw new NotFoundException($"Citation {citeId} was not found.");

        return citation;
    }

    #endregion

    #region Create Kami

    public async Task CreateKamiAsync(CreateKamiInShrineRequest request, string? publicId, CancellationToken ct)
    {
        // Create kami
        var kami = new Kami
        {
            NameEn = request.NameEn,
            NameJp = request.NameJp,
            Desc = request.Desc,
            Status = "draft"
        };

        // Create hero image if provided
        if (request.Image is not null)
        {
            var image = new Image
            {
                PublicId = publicId,
                ImageUrl = request.Image.ImageUrl,
                Title = request.Image.Title,
                Desc = request.Image.Desc
            };

            if (request.Image.Citation is not null)
            {
                image.Citation = new Citation
                {
                    Title = request.Image.Citation.Title,
                    Author = request.Image.Citation.Author,
                    Url = request.Image.Citation.Url,
                    Year = request.Image.Citation.Year
                };
            }

            kami.Image = image;
        }

        // Create new citations
        if (request.Citations.Create.Count > 0)
        {
            foreach (var citationRequest in request.Citations.Create)
            {
                var citation = BuildCitation(citationRequest);

                kami.KamiCitations.Add(new KamiCitation
                {
                    Kami = kami,
                    Citation = citation
                });
            }
        }

        // Link existing citations
        if (request.Citations.LinkExisting is not null && request.Citations.LinkExisting.Count > 0)
        {
            foreach (var citationRequest in request.Citations.LinkExisting)
            {
                var citation = await GetExistingCitationOrThrowAsync(citationRequest.CiteId, ct);

                // apply any edits sent along with the reuse request
                citation.Title = citationRequest.Title;
                citation.Author = citationRequest.Author;
                citation.Url = citationRequest.Url;
                citation.Year = citationRequest.Year;

                var alreadyLinked = kami.KamiCitations.Any(kc => kc.CiteId == citation.CiteId);
                if (alreadyLinked) continue;

                kami.KamiCitations.Add(new KamiCitation
                {
                    Kami = kami,
                    Citation = citation,
                    CiteId = citation.CiteId
                });
            }
        }

        _db.Kamis.Add(kami);

        await _db.SaveChangesAsync(ct);
    }

    #endregion

    #region Delete Kami

    public async Task DeleteKamiAsync(int kamiId, CancellationToken ct)
    {
        var kami = await _db.Kamis
            .Include(k => k.Image)
                .ThenInclude(i => i!.Citation)
            .Include(k => k.KamiCitations)
                .ThenInclude(kc => kc.Citation)
            .FirstOrDefaultAsync(k => k.KamiId == kamiId, ct);

        if (kami is null)
            throw new NotFoundException($"Kami {kamiId} was not found.");

        // Remove image citation only if nothing else uses it
        if (kami.Image?.Citation is not null)
        {
            var imageCitation = kami.Image.Citation;

            var isUsedElsewhere = await _db.Citations
                .Where(c => c.CiteId == imageCitation.CiteId)
                .AnyAsync(c =>
                    c.ImagesAttributed.Any(i => i.ImgId != kami.Image.ImgId) ||
                    c.KamiCitations.Any() ||
                    c.HistoryCitations.Any() ||
                    c.FolkloreCitations.Any(),
                    ct
                );

            if (!isUsedElsewhere)
                _db.Citations.Remove(imageCitation);
        }

        // Remove image
        if (kami.Image is not null)
        {
            _db.Images.Remove(kami.Image);
        }

        // Remove kami citation links, and delete citation only if unused elsewhere
        var kamiCitations = kami.KamiCitations.ToList();

        foreach (var kamiCitation in kamiCitations)
        {
            var citation = kamiCitation.Citation;

            _db.Set<KamiCitation>().Remove(kamiCitation);

            if (citation is null)
                continue;

            var isUsedElsewhere = await _db.Citations
                .Where(c => c.CiteId == citation.CiteId)
                .AnyAsync(c =>
                    c.KamiCitations.Any(kc => kc.KamiId != kamiId) ||
                    c.ImagesAttributed.Any() ||
                    c.HistoryCitations.Any() ||
                    c.FolkloreCitations.Any(),
                    ct
                );

            if (!isUsedElsewhere)
                _db.Citations.Remove(citation);
        }

        _db.Kamis.Remove(kami);

        await _db.SaveChangesAsync(ct);
    }

    #endregion

    #region Get All Kami

    public async Task<(IReadOnlyList<KamiReadCMSDto>, int)> GetAllKamiCMSAsync(GetAllKamiCMSQuery request, CancellationToken ct)
    {
        var query = _db.Kamis.AsNoTracking();

        // Search Query
        if (!string.IsNullOrWhiteSpace(request.SearchQuery))
        {
            var search = request.SearchQuery.Trim().ToLower();

            query = query.Where(t =>
                (t.NameEn != null && t.NameEn.ToLower().Contains(search)) ||
                (t.NameJp != null && t.NameJp.ToLower().Contains(search))
            );
        }

        // Sort Type / Direction
        var sort = request.Sort ?? TagsSort.IdAsc;
        query = sort switch
        {
            TagsSort.TitleAsc => query.OrderBy(t => t.NameEn),
            TagsSort.TitleDesc => query.OrderByDescending(t => t.NameEn),
            TagsSort.UpdatedAsc => query.OrderBy(t => t.UpdatedAt),
            TagsSort.UpdatedDesc => query.OrderByDescending(t => t.UpdatedAt),
            TagsSort.IdAsc => query.OrderBy(t => t.KamiId),
            TagsSort.IdDesc => query.OrderByDescending(t => t.KamiId),
            _ => query
        };

        // Get total count
        var totalCount = await query.CountAsync(ct);

        // Pagination
        var skip = (request.Page - 1) * request.PageSize;
        query = query
            .Skip(skip)
            .Take(request.PageSize);

        var items = await query
            .Select(k => new KamiReadCMSDto(
                k.KamiId,
                k.NameEn,
                k.NameJp,
                k.Desc,
                k.Status,
                k.PublishedAt,
                k.CreatedAt,
                k.UpdatedAt,
                    k.Image == null
                    ? null
                    : new ImageCMSDto(
                        k.Image.ImgId,
                        k.Image.ImageUrl,
                        k.Image.Title,
                        k.Image.Desc,
                        k.Image.Citation == null
                            ? null
                            : new CitationCMSDto(
                                k.Image.Citation.CiteId,
                                k.Image.Citation.Title,
                                k.Image.Citation.Author,
                                k.Image.Citation.Url,
                                k.Image.Citation.Year,
                                k.Image.Citation.CreatedAt,
                                k.Image.Citation.UpdatedAt
                            ),
                        k.Image.CreatedAt,
                        k.Image.UpdatedAt
                    ),
                k.KamiCitations
                    .Where(kc => kc.Citation != null)
                    .Select(kc => new CitationCMSDto(
                        kc.Citation.CiteId,
                        kc.Citation.Title,
                        kc.Citation.Author,
                        kc.Citation.Url,
                        kc.Citation.Year,
                        kc.Citation.CreatedAt,
                        kc.Citation.UpdatedAt
                    )).ToList(),
                null    // Nulling Audit
        )).ToListAsync(ct);

        return (items, totalCount);
    }

    #endregion

    #region Get PublicId

    public async Task<string?> GetKamiImagePublicIdCMSAsync(int kamiId, CancellationToken ct)
    {
        return await _db.Kamis
            .AsNoTracking()
            .Where(k => k.KamiId == kamiId)
            .Select(k => k.Image!.PublicId)
            .SingleOrDefaultAsync(ct);
    }

    #endregion

    #region Update Kami

    public async Task UpdateKamiAsync(int kamiId, UpdateKamiRequest request, string? publicId, CancellationToken ct)
    {
        // Load kami / related data
        var kami = await _db.Kamis
            .Include(k => k.Image!)
                .ThenInclude(i => i.Citation)
            .Include(k => k.KamiCitations)
                .ThenInclude(kc => kc.Citation)
            .FirstOrDefaultAsync(k => k.KamiId == kamiId, ct);

        // Validate kami exists
        if (kami is null)
            throw new NotFoundException("Kami not found.");

        // Update basic fields
        kami.NameEn = request.Basic.NameEn;
        kami.NameJp = request.Basic.NameJp;
        kami.Desc = request.Basic.Desc;

        // Delete image
        if (request.Image.Action == "delete")
        {
            kami.Image = null;
            kami.ImgId = null;
        }

        // Create image
        if (request.Image.Action == "create")
        {
            var image = new Image
            {
                PublicId = publicId,
                ImageUrl = request.Image.ImageUrl,
                Title = request.Image.Title,
                Desc = request.Image.Desc
            };

            if (request.Image.Citation is not null)
            {
                image.Citation = new Citation
                {
                    Title = request.Image.Citation.Title,
                    Author = request.Image.Citation.Author,
                    Url = request.Image.Citation.Url,
                    Year = request.Image.Citation.Year
                };
            }

            kami.Image = image;
        }

        // Update image
        if (request.Image.Action == "update")
        {
            if (kami.Image is null)
                throw new NotFoundException("Kami image not found.");

            if (!string.IsNullOrWhiteSpace(publicId))
            {
                kami.Image.PublicId = publicId;
            }
            kami.Image.ImageUrl = request.Image.ImageUrl;
            kami.Image.Title = request.Image.Title;
            kami.Image.Desc = request.Image.Desc;

            if (request.Image.Citation is null)
            {
                kami.Image.Citation = null;
                kami.Image.CiteId = null;
            }
            else if (kami.Image.Citation is null)
            {
                kami.Image.Citation = new Citation
                {
                    Title = request.Image.Citation.Title,
                    Author = request.Image.Citation.Author,
                    Url = request.Image.Citation.Url,
                    Year = request.Image.Citation.Year
                };
            }
            else
            {
                kami.Image.Citation.Title = request.Image.Citation.Title;
                kami.Image.Citation.Author = request.Image.Citation.Author;
                kami.Image.Citation.Url = request.Image.Citation.Url;
                kami.Image.Citation.Year = request.Image.Citation.Year;
            }
        }

        // Delete citations (unlink only)
        if (request.Citations.Delete.Count > 0)
        {
            var kamiCitationsToRemove = kami.KamiCitations
                .Where(kc => request.Citations.Delete.Contains(kc.CiteId))
                .ToList();

            foreach (var kamiCitation in kamiCitationsToRemove)
            {
                kami.KamiCitations.Remove(kamiCitation);
                _db.Set<KamiCitation>().Remove(kamiCitation);
            }
        }

        // Create citations
        if (request.Citations.Create.Count > 0)
        {
            foreach (var citationRequest in request.Citations.Create)
            {
                var citation = new Citation
                {
                    Title = citationRequest.Title,
                    Author = citationRequest.Author,
                    Url = citationRequest.Url,
                    Year = citationRequest.Year
                };

                kami.KamiCitations.Add(new KamiCitation
                {
                    KamiId = kami.KamiId,
                    Kami = kami,
                    Citation = citation
                });
            }
        }

        // Link existing citations
        if (request.Citations.LinkExisting.Count > 0)
        {
            foreach (var citationRequest in request.Citations.LinkExisting)
            {
                var citation = await GetExistingCitationOrThrowAsync(citationRequest.CiteId, ct);

                citation.Title = citationRequest.Title;
                citation.Author = citationRequest.Author;
                citation.Url = citationRequest.Url;
                citation.Year = citationRequest.Year;

                var alreadyLinked = kami.KamiCitations.Any(kc => kc.CiteId == citation.CiteId);
                if (alreadyLinked) continue;

                kami.KamiCitations.Add(new KamiCitation
                {
                    KamiId = kami.KamiId,
                    Kami = kami,
                    Citation = citation,
                    CiteId = citation.CiteId
                });
            }
        }

        // Update citations
        if (request.Citations.Update.Count > 0)
        {
            foreach (var citationRequest in request.Citations.Update)
            {
                var existingKamiCitation = kami.KamiCitations
                    .FirstOrDefault(kc => kc.CiteId == citationRequest.CiteId);

                if (existingKamiCitation is null)
                    continue;

                existingKamiCitation.Citation.Title = citationRequest.Title;
                existingKamiCitation.Citation.Author = citationRequest.Author;
                existingKamiCitation.Citation.Url = citationRequest.Url;
                existingKamiCitation.Citation.Year = citationRequest.Year;
            }
        }

        await _db.SaveChangesAsync(ct);
    }

    #endregion
}