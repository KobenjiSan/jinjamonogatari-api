using Application.Common.Models.Citations;
using Application.Common.Models.Images;
using Application.Features.Shrines.Models;
using Application.Features.Shrines.Services;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.Shrines;

public class ShrineReadService : IShrineReadService
{
    private readonly AppDbContext _db;

    public ShrineReadService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IReadOnlyList<ShrineMapPointDto>> GetShrineMapPointsAsync(CancellationToken ct)
    {
        return await _db.Shrines
            .AsNoTracking()
            .Where(s =>
                s.PublishedAt != null &&
                s.Slug != null &&
                s.Lat != null &&
                s.Lon != null)
            .Select(s => new ShrineMapPointDto(
                s.ShrineId,
                s.Slug!,
                s.Lat!.Value,
                s.Lon!.Value
            )).ToListAsync(ct);
    }

    public async Task<ShrinePreviewDto?> GetShrinePreviewAsync(string slug, CancellationToken ct)
    {
        return await _db.Shrines
            .AsNoTracking()
            .Where(s => s.Slug == slug && s.PublishedAt != null)
            .Select(s => new ShrinePreviewDto(
                s.ShrineId,
                s.Slug!,
                s.NameEn,
                s.NameJp,
                s.Image != null ? s.Image.ImgSource : null,
                s.ShrineDesc,
                s.ShrineTags
                    .Select(st => new TagDto(
                        st.TagId,
                        st.Tag.TitleEn,
                        st.Tag.TitleJp
                    )).ToList()
            )).SingleOrDefaultAsync(ct);
    }

    public async Task<IReadOnlyList<ShrineCardDto>> GetShrineListViewAsync(CancellationToken ct)
    {
        return await _db.Shrines
            .AsNoTracking()
            .Where(s => s.PublishedAt != null && s.Slug != null)  // Will need to filter by distance
            .Select(s => new ShrineCardDto(
                s.ShrineId,
                s.Slug!,
                s.NameEn,
                s.NameJp,
                s.Image != null ? s.Image.ImgSource : null
            )).ToListAsync(ct);
    }

    public async Task<ShrineMetaDto?> GetShrineMetaBySlugAsync(string slug, CancellationToken ct)
    {
        return await _db.Shrines
            .AsNoTracking()
            .Where(s => s.Slug == slug && s.PublishedAt != null)
            .Select(s => new ShrineMetaDto(
                s.ShrineId,
                s.Slug!,
                s.NameEn,
                s.NameJp,
                s.ShrineDesc,
                new AddressDto(
                    s.AddressRaw,
                    s.Prefecture,
                    s.City,
                    s.Ward,
                    s.Locality,
                    s.PostalCode,
                    s.Country
                ),
                s.PhoneNumber,
                s.Email,
                s.Website,
                s.Image != null ? s.Image.ImgSource : null,
                s.ShrineTags
                    .Select(st => new TagDto(
                    st.TagId,
                    st.Tag.TitleEn,
                    st.Tag.TitleJp
                )).ToList()
            )).SingleOrDefaultAsync(ct);
    }

    public async Task<IReadOnlyList<KamiReadDto>> GetShrineKamiBySlugAsync(string slug, CancellationToken ct)
    {
        return await _db.Shrines
            .AsNoTracking()
            .Where(s => s.Slug == slug && s.PublishedAt != null)
            .SelectMany(s => s.ShrineKamis.Select(sk => sk.Kami))
            .Where(k => k.PublishedAt != null)
            .Select(k => new KamiReadDto(
                k.KamiId,
                k.NameEn,
                k.NameJp,
                k.Desc,
                k.Image == null 
                    ? null
                    : new ImageCitedDto(
                        k.Image.ImgSource,
                        k.Image.Citation == null
                            ? null
                            : new CitationDto(
                                k.Image.Citation.CiteId,
                                k.Image.Citation.Title,
                                k.Image.Citation.Author,
                                k.Image.Citation.Url,
                                k.Image.Citation.Year
                            )
                    ),
                k.KamiCitations
                    .Select(kc => new CitationDto(
                        kc.Citation.CiteId,
                        kc.Citation.Title,
                        kc.Citation.Author,
                        kc.Citation.Url,
                        kc.Citation.Year
                        )
                    ).ToList()
            )).ToListAsync(ct);
    }

    public async Task<IReadOnlyList<HistoryReadDto>> GetShrineHistoryBySlugAsync(string slug, CancellationToken ct)
    {
        return await _db.Shrines
            .AsNoTracking()
            .Where(s => s.Slug == slug && s.PublishedAt != null)
            .SelectMany(s => s.ShrineHistories)
            .Where(h =>
                h.PublishedAt != null &&
                h.EventDate != null &&
                h.SortOrder != null &&
                h.Title != null)
            .OrderBy(h => h.SortOrder)
            .Select(h => new HistoryReadDto(
                h.HistoryId,
                h.EventDate!.Value,
                h.SortOrder!.Value,
                h.Title!,
                h.Information,
                h.Image == null 
                    ? null
                    : new ImageCitedDto(
                        h.Image.ImgSource,
                        h.Image.Citation == null
                            ? null
                            : new CitationDto(
                                h.Image.Citation.CiteId,
                                h.Image.Citation.Title,
                                h.Image.Citation.Author,
                                h.Image.Citation.Url,
                                h.Image.Citation.Year
                            )
                    ),
                h.HistoryCitations
                    .Select(hc => new CitationDto(
                        hc.Citation.CiteId,
                        hc.Citation.Title,
                        hc.Citation.Author,
                        hc.Citation.Url,
                        hc.Citation.Year
                    )).ToList()
            )).ToListAsync(ct);
    }

    public async Task<IReadOnlyList<FolkloreReadDto>> GetShrineFolkloreBySlugAsync(string slug, CancellationToken ct)
    {
        return await _db.Shrines
            .AsNoTracking()
            .Where(s => s.Slug == slug && s.PublishedAt != null)
            .SelectMany(s => s.ShrineFolklores)
            .Where(f =>
                f.PublishedAt != null &&
                f.Title != null &&
                f.Information != null)
            .OrderBy(f => f.SortOrder)
            .Select(f => new FolkloreReadDto(
                f.FolkloreId,
                f.Title!,
                f.Information!,
                f.Image == null 
                    ? null
                    : new ImageCitedDto(
                        f.Image.ImgSource,
                        f.Image.Citation == null
                            ? null
                            : new CitationDto(
                                f.Image.Citation.CiteId,
                                f.Image.Citation.Title,
                                f.Image.Citation.Author,
                                f.Image.Citation.Url,
                                f.Image.Citation.Year
                            )
                    ),
                f.FolkloreCitations
                    .Select(fc => new CitationDto(
                        fc.Citation.CiteId,
                        fc.Citation.Title,
                        fc.Citation.Author,
                        fc.Citation.Url,
                        fc.Citation.Year
                    )).ToList()
            )).ToListAsync(ct);
    }

    public async Task<IReadOnlyList<GalleryListItemDto>> GetShrineGalleryBySlugAsync(string slug, CancellationToken ct)
    {
        return await _db.Shrines
            .AsNoTracking()
            .Where(s => s.Slug == slug && s.PublishedAt != null)
            .SelectMany(s => s.ShrineGalleries.Select(sg => sg.Image))
            .Where(i => i.ImgSource != null)
            .Select(i => new GalleryListItemDto(
                i.ImgId,
                i.ImgSource!
            )).ToListAsync(ct);
    }

    public async Task<ImageFullDto?> GetImageByIdAsync(int id, CancellationToken ct)
    {
        return await _db.Images
            .AsNoTracking()
            .Where(i => i.ImgId == id)
            .Select(i => new ImageFullDto(
                i.ImgId,
                i.ImgSource,
                i.Title,
                i.Desc,
                i.Citation == null
                    ? null
                    : new CitationDto(
                        i.Citation.CiteId,
                        i.Citation.Title,
                        i.Citation.Author,
                        i.Citation.Url,
                        i.Citation.Year
                    )
            )).SingleOrDefaultAsync(ct);
    }
}