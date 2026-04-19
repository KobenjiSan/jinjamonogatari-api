using Application.Common.Models.Citations;
using Application.Common.Models.Images;
using Application.Features.Shrines.Models;
using Application.Features.Shrines.Queries.GetShrineListCMS;
using Application.Features.Shrines.Services;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;

namespace Infrastructure.Services.Shrines;

public class ShrineReadService : IShrineReadService
{
    private readonly AppDbContext _db;

    public ShrineReadService(AppDbContext db)
    {
        _db = db;
    }

    #region Shrine Map Points

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

    #endregion

    #region Shrine Preview

    public async Task<ShrinePreviewDto?> GetShrinePreviewAsync(
        string slug,
        double? lat,
        double? lon,
        CancellationToken ct
    )
    {
        var hasUserPoint = lat.HasValue && lon.HasValue;

        var userPoint = hasUserPoint
            ? new Point(lon!.Value, lat!.Value) { SRID = 4326 }
            : null;

        return await _db.Shrines
            .AsNoTracking()
            .Where(s => s.Slug == slug && s.PublishedAt != null)
            .Select(s => new ShrinePreviewDto(
                s.ShrineId,
                s.Slug!,
                s.Lat.HasValue ? (double?)s.Lat.Value : null,
                s.Lon.HasValue ? (double?)s.Lon.Value : null,
                s.NameEn,
                s.NameJp,
                s.Image != null ? s.Image.ImageUrl : null,
                s.ShrineDesc,
                (hasUserPoint && s.Location != null)
                    ? EF.Functions.Distance(s.Location!, userPoint!, true)
                    : (double?)null,
                s.ShrineTags
                    .Select(st => new TagDto(
                        st.TagId,
                        st.Tag.TitleEn,
                        st.Tag.TitleJp
                    )).ToList()
            )).SingleOrDefaultAsync(ct);
    }

    #endregion

    #region Shrine List View

    public async Task<IReadOnlyList<ShrineCardDto>> GetShrineListViewAsync(
        double? lat,
        double? lon,
        string? q,
        CancellationToken ct
    )
    {
        var hasUserPoint = lat.HasValue && lon.HasValue;

        var userPoint = hasUserPoint
            ? new Point(lon!.Value, lat!.Value) { SRID = 4326 }
            : null;

        var hasQ = !string.IsNullOrWhiteSpace(q);
        var pattern = hasQ ? $"%{q!.Trim()}%" : null;

        return await _db.Shrines
            .AsNoTracking()
            .Where(s => s.PublishedAt != null && s.Slug != null)
            .Where(s => !hasQ || (
                EF.Functions.ILike(s.NameEn ?? "", pattern!) ||
                EF.Functions.ILike(s.NameJp ?? "", pattern!) ||
                EF.Functions.ILike(s.Slug ?? "", pattern!) ||
                EF.Functions.ILike(s.ShrineDesc ?? "", pattern!) ||
                EF.Functions.ILike(s.Prefecture ?? "", pattern!) ||
                EF.Functions.ILike(s.City ?? "", pattern!) ||
                EF.Functions.ILike(s.Ward ?? "", pattern!) ||
                EF.Functions.ILike(s.Locality ?? "", pattern!) ||
                EF.Functions.ILike(s.AddressRaw ?? "", pattern!) ||
                EF.Functions.ILike(s.PostalCode ?? "", pattern!) ||
                EF.Functions.ILike(s.Country ?? "", pattern!)
            ))
            .Select(s => new ShrineCardDto(
                s.ShrineId,
                s.Slug!,
                s.NameEn,
                s.NameJp,
                s.Image != null ? s.Image.ImageUrl : null,
                (hasUserPoint && s.Location != null)
                    ? EF.Functions.Distance(s.Location!, userPoint!, true)
                    : (double?)null
            ))
            .ToListAsync(ct);
    }

    #endregion

    #region Shrine Meta 

    public async Task<ShrineMetaDto?> GetShrineMetaBySlugAsync(
        string slug,
        double? lat,
        double? lon,
        CancellationToken ct
    )
    {
        var hasUserPoint = lat.HasValue && lon.HasValue;

        var userPoint = hasUserPoint
            ? new Point(lon!.Value, lat!.Value) { SRID = 4326 }
            : null;

        return await _db.Shrines
            .AsNoTracking()
            .Where(s => s.Slug == slug && s.PublishedAt != null)
            .Select(s => new ShrineMetaDto(
                s.ShrineId,
                s.Slug!,
                s.Lat.HasValue ? (double?)s.Lat.Value : null,
                s.Lon.HasValue ? (double?)s.Lon.Value : null,
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
                s.Image != null ? s.Image.ImageUrl : null,
                (hasUserPoint && s.Location != null)
                    ? EF.Functions.Distance(s.Location!, userPoint!, true)
                    : (double?)null,
                s.ShrineTags
                    .Select(st => new TagDto(
                        st.TagId,
                        st.Tag.TitleEn,
                        st.Tag.TitleJp
                    )).ToList()
            )).SingleOrDefaultAsync(ct);
    }

    #endregion

    #region Shrine Kami

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
                        k.Image.ImageUrl,
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

    #endregion

    #region Shrine History

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
                        h.Image.ImageUrl,
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

    #endregion

    #region Shrine Folklore

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
                        f.Image.ImageUrl,
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

    #endregion

    #region Shrine Gallery

    public async Task<IReadOnlyList<GalleryListItemDto>> GetShrineGalleryBySlugAsync(string slug, CancellationToken ct)
    {
        return await _db.Shrines
            .AsNoTracking()
            .Where(s => s.Slug == slug && s.PublishedAt != null)
            .SelectMany(s => s.ShrineGalleries.Select(sg => sg.Image))
            .Where(i => i.ImageUrl != null)
            .Select(i => new GalleryListItemDto(
                i.ImgId,
                i.ImageUrl!
            )).ToListAsync(ct);
    }

    #endregion

    #region Image by Id

    public async Task<ImageFullDto?> GetImageByIdAsync(int id, CancellationToken ct)
    {
        return await _db.Images
            .AsNoTracking()
            .Where(i => i.ImgId == id)
            .Select(i => new ImageFullDto(
                i.ImgId,
                i.ImageUrl,
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

    #endregion

    #region Does Shrine Exist

    public Task<bool> DoesShrineExistAsync(int shrineId, CancellationToken ct)
        => _db.Shrines
            .AsNoTracking()
            .AnyAsync(s => s.ShrineId == shrineId, ct);

    #endregion

    #region CMS Shrine List

    public async Task<(IReadOnlyList<ShrineListCMSDto>, int)> GetShrineListCMSAsync(GetShrineListCMSQuery request, CancellationToken ct)
    {
        var query = _db.Shrines
            .AsNoTracking();

        // Sort Status
        if (!string.IsNullOrWhiteSpace(request.Status))
        {
            query = query.Where(s => s.Status == request.Status);
        }

        // Sort Prefecture
        if (!string.IsNullOrWhiteSpace(request.Prefecture))
        {
            query = query.Where(s =>
                s.Prefecture != null && s.Prefecture.ToLower() == request.Prefecture.ToLower()
            );
        }

        // Search Query
        if (!string.IsNullOrWhiteSpace(request.SearchQuery))
        {
            var search = request.SearchQuery.Trim().ToLower();

            query = query.Where(s =>
                (s.NameEn != null && s.NameEn.ToLower().Contains(search)) ||
                (s.NameJp != null && s.NameJp.ToLower().Contains(search)) ||
                (s.ShrineDesc != null && s.ShrineDesc.ToLower().Contains(search))
            );
        }

        // Sort Type / Direction
        var sort = request.Sort ?? ShrineSort.UpdatedDesc;
        query = sort switch
        {
            ShrineSort.TitleAsc => query.OrderBy(s => s.NameEn),
            ShrineSort.TitleDesc => query.OrderByDescending(s => s.NameEn),
            ShrineSort.UpdatedAsc => query.OrderBy(s => s.UpdatedAt),
            ShrineSort.UpdatedDesc => query.OrderByDescending(s => s.UpdatedAt),
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
            .Select(s => new ShrineListCMSDto(
                s.ShrineId,
                s.NameEn,
                s.NameJp,
                s.Status,
                s.City,
                s.Lat,
                s.Lon,
                s.UpdatedAt,
                null,
                false
            )).ToListAsync(ct);

        return (items, totalCount);
    }

    #endregion

    #region CMS Shrine Meta

    public async Task<ShrineMetaCMSDto?> GetShrineMetaByIdCMSAsync(int id, CancellationToken ct)
    {
        return await _db.Shrines
            .AsNoTracking()
            .Where(s => s.ShrineId == id)
            .Select(s => new ShrineMetaCMSDto(
                s.ShrineId,
                s.InputtedId ?? string.Empty,
                s.Slug,
                s.NameEn,
                s.NameJp,
                s.ShrineDesc,
                s.Lat,
                s.Lon,
                s.Prefecture,
                s.City,
                s.Ward,
                s.Locality,
                s.PostalCode,
                s.Country,
                s.PhoneNumber,
                s.Email,
                s.Website,
                s.Image == null
                    ? null
                    : new ImageFullDto(
                        s.Image.ImgId,
                        s.Image.ImageUrl,
                        s.Image.Title,
                        s.Image.Desc,
                        s.Image.Citation == null
                            ? null
                            : new CitationDto(
                                s.Image.Citation.CiteId,
                                s.Image.Citation.Title,
                                s.Image.Citation.Author,
                                s.Image.Citation.Url,
                                s.Image.Citation.Year
                            )
                    ),
                s.Status ?? "Draft",
                s.PublishedAt,
                s.CreatedAt,
                s.UpdatedAt,
                s.Notes,
                s.ShrineTags
                    .Select(st => new TagDto(
                        st.TagId,
                        st.Tag.TitleEn,
                        st.Tag.TitleJp
                    )).ToList()
            )).SingleOrDefaultAsync(ct);
    }

    #endregion

    #region CMS Shrine Notes

    public async Task<string?> GetShrineNotesByIdCMSAsync(int id, CancellationToken ct)
    {
        return await _db.Shrines
            .AsNoTracking()
            .Where(s => s.ShrineId == id)
            .Select(s => s.Notes).SingleOrDefaultAsync(ct);
    }

    #endregion

    #region CMS Shrine Kami

    public async Task<IReadOnlyList<KamiReadCMSDto>> GetShrineKamiByIdCMSAsync(int id, CancellationToken ct)
    {
        return await _db.Shrines
            .AsNoTracking()
            .Where(s => s.ShrineId == id)
            .SelectMany(s => s.ShrineKamis.Select(sk => sk.Kami))
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
    }

    #endregion

    #region CMS All Kami List

    public async Task<IReadOnlyList<KamiReadCMSDto>> GetAllKamiListCMSAsync(CancellationToken ct)
    {
        return await _db.Kamis
            .AsNoTracking()
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
    }

    #endregion

    #region CMS Shrine History

    public async Task<IReadOnlyList<HistoryReadCMSDto>> GetShrineHistoryByIdCMSAsync(int id, CancellationToken ct)
    {
        return await _db.Histories
           .AsNoTracking()
           .Where(h => h.ShrineId == id)
           .OrderBy(h => h.SortOrder)
           .ThenBy(h => h.EventDate)
           .Select(h => new HistoryReadCMSDto(
               h.HistoryId,
               h.EventDate,
               h.SortOrder,
               h.Title,
               h.Information,
               h.Status,
               h.PublishedAt,
               h.CreatedAt,
               h.UpdatedAt,
               h.Image == null
               ? null
               : new ImageCMSDto(
                   h.Image.ImgId,
                   h.Image.ImageUrl,
                   h.Image.Title,
                   h.Image.Desc,
                   h.Image.Citation == null
                       ? null
                       : new CitationCMSDto(
                           h.Image.Citation.CiteId,
                           h.Image.Citation.Title,
                           h.Image.Citation.Author,
                           h.Image.Citation.Url,
                           h.Image.Citation.Year,
                           h.Image.Citation.CreatedAt,
                           h.Image.Citation.UpdatedAt
                       ),
                   h.Image.CreatedAt,
                   h.Image.UpdatedAt
               ),
               h.HistoryCitations
               .Where(hc => hc.Citation != null)
               .Select(hc => new CitationCMSDto(
                   hc.Citation.CiteId,
                   hc.Citation.Title,
                   hc.Citation.Author,
                   hc.Citation.Url,
                   hc.Citation.Year,
                   hc.Citation.CreatedAt,
                   hc.Citation.UpdatedAt
               )).ToList(),
               null    // Nulling Audit
           )).ToListAsync(ct);
    }

    #endregion

    #region CMS Shrine Folklore

    public async Task<IReadOnlyList<FolkloreReadCMSDto>> GetShrineFolkloreByIdCMSAsync(int id, CancellationToken ct)
    {
        return await _db.Folklores
           .AsNoTracking()
           .Where(h => h.ShrineId == id)
           .OrderBy(h => h.SortOrder)
           .Select(h => new FolkloreReadCMSDto(
               h.FolkloreId,
               h.SortOrder,
               h.Title,
               h.Information,
               h.Status,
               h.PublishedAt,
               h.CreatedAt,
               h.UpdatedAt,
               h.Image == null
               ? null
               : new ImageCMSDto(
                   h.Image.ImgId,
                   h.Image.ImageUrl,
                   h.Image.Title,
                   h.Image.Desc,
                   h.Image.Citation == null
                       ? null
                       : new CitationCMSDto(
                           h.Image.Citation.CiteId,
                           h.Image.Citation.Title,
                           h.Image.Citation.Author,
                           h.Image.Citation.Url,
                           h.Image.Citation.Year,
                           h.Image.Citation.CreatedAt,
                           h.Image.Citation.UpdatedAt
                       ),
                   h.Image.CreatedAt,
                   h.Image.UpdatedAt
               ),
               h.FolkloreCitations
               .Where(hc => hc.Citation != null)
               .Select(hc => new CitationCMSDto(
                   hc.Citation.CiteId,
                   hc.Citation.Title,
                   hc.Citation.Author,
                   hc.Citation.Url,
                   hc.Citation.Year,
                   hc.Citation.CreatedAt,
                   hc.Citation.UpdatedAt
               )).ToList(),
               null    // Nulling Audit
           )).ToListAsync(ct);
    }

    #endregion

    #region CMS Shrine Gallery

    public async Task<IReadOnlyList<ImageCMSDto>> GetShrineGalleryByIdCMSAsync(int id, CancellationToken ct)
    {
        return await _db.Shrines
            .AsNoTracking()
            .Where(s => s.ShrineId == id)
            .SelectMany(s => s.ShrineGalleries.Select(sg => sg.Image))
            .Select(i => new ImageCMSDto(
               i.ImgId,
                i.ImageUrl,
                i.Title,
                i.Desc,
                i.Citation == null
                    ? null
                    : new CitationCMSDto(
                        i.Citation.CiteId,
                        i.Citation.Title,
                        i.Citation.Author,
                        i.Citation.Url,
                        i.Citation.Year,
                        i.Citation.CreatedAt,
                        i.Citation.UpdatedAt
                    ),
                i.CreatedAt,
                i.UpdatedAt
            )).ToListAsync(ct);
    }

    #endregion

    #region CMS Shrine Audit

    public async Task<ShrineAuditSnapshot?> GetShrineAuditSnapshotAsync(int shrineId, CancellationToken ct)
    {
        return await _db.Shrines
            .AsNoTracking()
            .Where(s => s.ShrineId == shrineId)
            .Select(s => new ShrineAuditSnapshot
            {
                ShrineId = s.ShrineId,
                InputtedId = s.InputtedId,
                Slug = s.Slug,
                NameEn = s.NameEn,
                NameJp = s.NameJp,
                ShrineDesc = s.ShrineDesc,
                Lat = s.Lat,
                Lon = s.Lon,

                Prefecture = s.Prefecture,
                City = s.City,
                Ward = s.Ward,
                Locality = s.Locality,
                PostalCode = s.PostalCode,
                Country = s.Country,

                PhoneNumber = s.PhoneNumber,
                Email = s.Email,
                Website = s.Website,

                HeroImage = s.Image == null ? null : new ImageAuditSnapshot
                {
                    ImgId = s.Image.ImgId,
                    ImageUrl = s.Image.ImageUrl,
                    Title = s.Image.Title,
                    Desc = s.Image.Desc,
                    Citation = s.Image.Citation == null ? null : new CitationAuditSnapshot
                    {
                        CiteId = s.Image.Citation.CiteId,
                        Title = s.Image.Citation.Title,
                        Author = s.Image.Citation.Author,
                        Url = s.Image.Citation.Url,
                        Year = s.Image.Citation.Year
                    }
                },

                Tags = s.ShrineTags
                    .Select(st => new TagAuditSnapshot
                    {
                        TagId = st.Tag.TagId,
                        TitleEn = st.Tag.TitleEn,
                        TitleJp = st.Tag.TitleJp
                    })
                    .ToList(),

                Kami = s.ShrineKamis
                    .Select(sk => new KamiAuditSnapshot
                    {
                        KamiId = sk.Kami.KamiId,
                        NameEn = sk.Kami.NameEn,
                        NameJp = sk.Kami.NameJp,
                        Desc = sk.Kami.Desc,
                        Image = sk.Kami.Image == null ? null : new ImageAuditSnapshot
                        {
                            ImgId = sk.Kami.Image.ImgId,
                            ImageUrl = sk.Kami.Image.ImageUrl,
                            Title = sk.Kami.Image.Title,
                            Desc = sk.Kami.Image.Desc,
                            Citation = sk.Kami.Image.Citation == null ? null : new CitationAuditSnapshot
                            {
                                CiteId = sk.Kami.Image.Citation.CiteId,
                                Title = sk.Kami.Image.Citation.Title,
                                Author = sk.Kami.Image.Citation.Author,
                                Url = sk.Kami.Image.Citation.Url,
                                Year = sk.Kami.Image.Citation.Year
                            }
                        },
                        Citations = sk.Kami.KamiCitations
                            .Select(kc => new CitationAuditSnapshot
                            {
                                CiteId = kc.Citation.CiteId,
                                Title = kc.Citation.Title,
                                Author = kc.Citation.Author,
                                Url = kc.Citation.Url,
                                Year = kc.Citation.Year
                            })
                            .ToList()
                    })
                    .ToList(),

                Histories = s.ShrineHistories
                    .Select(h => new HistoryAuditSnapshot
                    {
                        HistoryId = h.HistoryId,
                        EventDate = h.EventDate,
                        SortOrder = h.SortOrder,
                        Title = h.Title,
                        Information = h.Information,
                        Image = h.Image == null ? null : new ImageAuditSnapshot
                        {
                            ImgId = h.Image.ImgId,
                            ImageUrl = h.Image.ImageUrl,
                            Title = h.Image.Title,
                            Desc = h.Image.Desc,
                            Citation = h.Image.Citation == null ? null : new CitationAuditSnapshot
                            {
                                CiteId = h.Image.Citation.CiteId,
                                Title = h.Image.Citation.Title,
                                Author = h.Image.Citation.Author,
                                Url = h.Image.Citation.Url,
                                Year = h.Image.Citation.Year
                            }
                        },
                        Citations = h.HistoryCitations
                            .Select(hc => new CitationAuditSnapshot
                            {
                                CiteId = hc.Citation.CiteId,
                                Title = hc.Citation.Title,
                                Author = hc.Citation.Author,
                                Url = hc.Citation.Url,
                                Year = hc.Citation.Year
                            })
                            .ToList()
                    })
                    .ToList(),

                Folklores = s.ShrineFolklores
                    .Select(f => new FolkloreAuditSnapshot
                    {
                        FolkloreId = f.FolkloreId,
                        SortOrder = f.SortOrder,
                        Title = f.Title,
                        Information = f.Information,
                        Image = f.Image == null ? null : new ImageAuditSnapshot
                        {
                            ImgId = f.Image.ImgId,
                            ImageUrl = f.Image.ImageUrl,
                            Title = f.Image.Title,
                            Desc = f.Image.Desc,
                            Citation = f.Image.Citation == null ? null : new CitationAuditSnapshot
                            {
                                CiteId = f.Image.Citation.CiteId,
                                Title = f.Image.Citation.Title,
                                Author = f.Image.Citation.Author,
                                Url = f.Image.Citation.Url,
                                Year = f.Image.Citation.Year
                            }
                        },
                        Citations = f.FolkloreCitations
                            .Select(fc => new CitationAuditSnapshot
                            {
                                CiteId = fc.Citation.CiteId,
                                Title = fc.Citation.Title,
                                Author = fc.Citation.Author,
                                Url = fc.Citation.Url,
                                Year = fc.Citation.Year
                            })
                            .ToList()
                    })
                    .ToList(),

                GalleryImages = s.ShrineGalleries
                    .Select(g => new ImageAuditSnapshot
                    {
                        ImgId = g.Image.ImgId,
                        ImageUrl = g.Image.ImageUrl,
                        Title = g.Image.Title,
                        Desc = g.Image.Desc,
                        Citation = g.Image.Citation == null ? null : new CitationAuditSnapshot
                        {
                            CiteId = g.Image.Citation.CiteId,
                            Title = g.Image.Citation.Title,
                            Author = g.Image.Citation.Author,
                            Url = g.Image.Citation.Url,
                            Year = g.Image.Citation.Year
                        }
                    })
                    .ToList()
            })
            .FirstOrDefaultAsync(ct);
    }

    #endregion

    #region CMS Shrine Citations

    public async Task<List<ShrineCitationCMSDto>> GetShrineCitationsByIdCMSAsync(int shrineId, CancellationToken ct)
    {
        var shrineData = await _db.Shrines
            .AsNoTracking()
            .Where(s => s.ShrineId == shrineId)
            .Select(s => new
            {
                HeroImage = s.Image == null
                    ? null
                    : new
                    {
                        Citation = s.Image.Citation == null
                            ? null
                            : new CitationCMSDto(
                                s.Image.Citation.CiteId,
                                s.Image.Citation.Title,
                                s.Image.Citation.Author,
                                s.Image.Citation.Url,
                                s.Image.Citation.Year,
                                s.Image.Citation.CreatedAt,
                                s.Image.Citation.UpdatedAt
                            )
                    },

                Kami = s.ShrineKamis.Select(sk => new
                {
                    KamiId = sk.Kami.KamiId,
                    Name = sk.Kami.NameEn ?? sk.Kami.NameJp ?? $"Kami #{sk.Kami.KamiId}",

                    ImageCitation = sk.Kami.Image == null || sk.Kami.Image.Citation == null
                        ? null
                        : new CitationCMSDto(
                            sk.Kami.Image.Citation.CiteId,
                            sk.Kami.Image.Citation.Title,
                            sk.Kami.Image.Citation.Author,
                            sk.Kami.Image.Citation.Url,
                            sk.Kami.Image.Citation.Year,
                            sk.Kami.Image.Citation.CreatedAt,
                            sk.Kami.Image.Citation.UpdatedAt
                        ),

                    Citations = sk.Kami.KamiCitations
                        .Select(kc => new CitationCMSDto(
                            kc.Citation.CiteId,
                            kc.Citation.Title,
                            kc.Citation.Author,
                            kc.Citation.Url,
                            kc.Citation.Year,
                            kc.Citation.CreatedAt,
                            kc.Citation.UpdatedAt
                        ))
                        .ToList()
                }).ToList(),

                Histories = s.ShrineHistories.Select(h => new
                {
                    HistoryId = h.HistoryId,
                    Name = h.Title ?? $"History #{h.HistoryId}",

                    ImageCitation = h.Image == null || h.Image.Citation == null
                        ? null
                        : new CitationCMSDto(
                            h.Image.Citation.CiteId,
                            h.Image.Citation.Title,
                            h.Image.Citation.Author,
                            h.Image.Citation.Url,
                            h.Image.Citation.Year,
                            h.Image.Citation.CreatedAt,
                            h.Image.Citation.UpdatedAt
                        ),

                    Citations = h.HistoryCitations
                        .Select(hc => new CitationCMSDto(
                            hc.Citation.CiteId,
                            hc.Citation.Title,
                            hc.Citation.Author,
                            hc.Citation.Url,
                            hc.Citation.Year,
                            hc.Citation.CreatedAt,
                            hc.Citation.UpdatedAt
                        ))
                        .ToList()
                }).ToList(),

                Folklores = s.ShrineFolklores.Select(f => new
                {
                    FolkloreId = f.FolkloreId,
                    Name = f.Title ?? $"Folklore #{f.FolkloreId}",

                    ImageCitation = f.Image == null || f.Image.Citation == null
                        ? null
                        : new CitationCMSDto(
                            f.Image.Citation.CiteId,
                            f.Image.Citation.Title,
                            f.Image.Citation.Author,
                            f.Image.Citation.Url,
                            f.Image.Citation.Year,
                            f.Image.Citation.CreatedAt,
                            f.Image.Citation.UpdatedAt
                        ),

                    Citations = f.FolkloreCitations
                        .Select(fc => new CitationCMSDto(
                            fc.Citation.CiteId,
                            fc.Citation.Title,
                            fc.Citation.Author,
                            fc.Citation.Url,
                            fc.Citation.Year,
                            fc.Citation.CreatedAt,
                            fc.Citation.UpdatedAt
                        ))
                        .ToList()
                }).ToList(),

                GalleryImages = s.ShrineGalleries.Select(g => new
                {
                    ImageId = g.Image.ImgId,
                    Name = g.Image.Title ?? $"Gallery Image #{g.Image.ImgId}",

                    Citation = g.Image.Citation == null
                        ? null
                        : new CitationCMSDto(
                            g.Image.Citation.CiteId,
                            g.Image.Citation.Title,
                            g.Image.Citation.Author,
                            g.Image.Citation.Url,
                            g.Image.Citation.Year,
                            g.Image.Citation.CreatedAt,
                            g.Image.Citation.UpdatedAt
                        )
                }).ToList()
            })
            .FirstOrDefaultAsync(ct);

        if (shrineData == null)
            return new List<ShrineCitationCMSDto>();

        var usages = new List<(CitationCMSDto Citation, CitationLinkedItemDto LinkedItem)>();

        if (shrineData.HeroImage?.Citation != null)
        {
            usages.Add((
                shrineData.HeroImage.Citation,
                new CitationLinkedItemDto("ShrineMeta", shrineId, "Hero Image")
            ));
        }

        foreach (var kami in shrineData.Kami)
        {
            if (kami.ImageCitation != null)
            {
                usages.Add((
                    kami.ImageCitation,
                    new CitationLinkedItemDto("KamiImage", kami.KamiId, kami.Name)
                ));
            }

            foreach (var citation in kami.Citations)
            {
                usages.Add((
                    citation,
                    new CitationLinkedItemDto("Kami", kami.KamiId, kami.Name)
                ));
            }
        }

        foreach (var history in shrineData.Histories)
        {
            if (history.ImageCitation != null)
            {
                usages.Add((
                    history.ImageCitation,
                    new CitationLinkedItemDto("HistoryImage", history.HistoryId, history.Name)
                ));
            }

            foreach (var citation in history.Citations)
            {
                usages.Add((
                    citation,
                    new CitationLinkedItemDto("History", history.HistoryId, history.Name)
                ));
            }
        }

        foreach (var folklore in shrineData.Folklores)
        {
            if (folklore.ImageCitation != null)
            {
                usages.Add((
                    folklore.ImageCitation,
                    new CitationLinkedItemDto("FolkloreImage", folklore.FolkloreId, folklore.Name)
                ));
            }

            foreach (var citation in folklore.Citations)
            {
                usages.Add((
                    citation,
                    new CitationLinkedItemDto("Folklore", folklore.FolkloreId, folklore.Name)
                ));
            }
        }

        foreach (var galleryImage in shrineData.GalleryImages)
        {
            if (galleryImage.Citation != null)
            {
                usages.Add((
                    galleryImage.Citation,
                    new CitationLinkedItemDto("GalleryImage", galleryImage.ImageId, galleryImage.Name)
                ));
            }
        }

        return usages
            .GroupBy(x => x.Citation.CiteId)
            .Select(g => new ShrineCitationCMSDto(
                g.First().Citation,
                g.Count(),
                g.Select(x => x.LinkedItem)
                 .Distinct()
                 .ToList()
            ))
            .OrderBy(x => x.Citation.Title ?? $"Citation {x.Citation.CiteId}")
            .ToList();
    }

    #endregion

    #region CMS Shrine Citations Dropdown

    public async Task<List<CitationCMSDto>> GetShrineCitationsDropdownByIdCMSAsync(
    int shrineId,
    CancellationToken ct)
    {
        var shrineData = await _db.Shrines
            .AsNoTracking()
            .Where(s => s.ShrineId == shrineId)
            .Select(s => new
            {
                HeroImageCitation = s.Image == null || s.Image.Citation == null
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

                KamiImageCitations = s.ShrineKamis
                    .Where(sk => sk.Kami.Image != null && sk.Kami.Image.Citation != null)
                    .Select(sk => new CitationCMSDto(
                        sk.Kami.Image!.Citation!.CiteId,
                        sk.Kami.Image.Citation.Title,
                        sk.Kami.Image.Citation.Author,
                        sk.Kami.Image.Citation.Url,
                        sk.Kami.Image.Citation.Year,
                        sk.Kami.Image.Citation.CreatedAt,
                        sk.Kami.Image.Citation.UpdatedAt
                    ))
                    .ToList(),

                KamiCitations = s.ShrineKamis
                    .SelectMany(sk => sk.Kami.KamiCitations)
                    .Select(kc => new CitationCMSDto(
                        kc.Citation.CiteId,
                        kc.Citation.Title,
                        kc.Citation.Author,
                        kc.Citation.Url,
                        kc.Citation.Year,
                        kc.Citation.CreatedAt,
                        kc.Citation.UpdatedAt
                    ))
                    .ToList(),

                HistoryImageCitations = s.ShrineHistories
                    .Where(h => h.Image != null && h.Image.Citation != null)
                    .Select(h => new CitationCMSDto(
                        h.Image!.Citation!.CiteId,
                        h.Image.Citation.Title,
                        h.Image.Citation.Author,
                        h.Image.Citation.Url,
                        h.Image.Citation.Year,
                        h.Image.Citation.CreatedAt,
                        h.Image.Citation.UpdatedAt
                    ))
                    .ToList(),

                HistoryCitations = s.ShrineHistories
                    .SelectMany(h => h.HistoryCitations)
                    .Select(hc => new CitationCMSDto(
                        hc.Citation.CiteId,
                        hc.Citation.Title,
                        hc.Citation.Author,
                        hc.Citation.Url,
                        hc.Citation.Year,
                        hc.Citation.CreatedAt,
                        hc.Citation.UpdatedAt
                    ))
                    .ToList(),

                FolkloreImageCitations = s.ShrineFolklores
                    .Where(f => f.Image != null && f.Image.Citation != null)
                    .Select(f => new CitationCMSDto(
                        f.Image!.Citation!.CiteId,
                        f.Image.Citation.Title,
                        f.Image.Citation.Author,
                        f.Image.Citation.Url,
                        f.Image.Citation.Year,
                        f.Image.Citation.CreatedAt,
                        f.Image.Citation.UpdatedAt
                    ))
                    .ToList(),

                FolkloreCitations = s.ShrineFolklores
                    .SelectMany(f => f.FolkloreCitations)
                    .Select(fc => new CitationCMSDto(
                        fc.Citation.CiteId,
                        fc.Citation.Title,
                        fc.Citation.Author,
                        fc.Citation.Url,
                        fc.Citation.Year,
                        fc.Citation.CreatedAt,
                        fc.Citation.UpdatedAt
                    ))
                    .ToList(),

                GalleryImageCitations = s.ShrineGalleries
                    .Where(g => g.Image.Citation != null)
                    .Select(g => new CitationCMSDto(
                        g.Image.Citation!.CiteId,
                        g.Image.Citation.Title,
                        g.Image.Citation.Author,
                        g.Image.Citation.Url,
                        g.Image.Citation.Year,
                        g.Image.Citation.CreatedAt,
                        g.Image.Citation.UpdatedAt
                    ))
                    .ToList()
            })
            .FirstOrDefaultAsync(ct);

        if (shrineData == null)
            return new List<CitationCMSDto>();

        var citations = new List<CitationCMSDto>();

        if (shrineData.HeroImageCitation != null)
            citations.Add(shrineData.HeroImageCitation);

        citations.AddRange(shrineData.KamiImageCitations);
        citations.AddRange(shrineData.KamiCitations);
        citations.AddRange(shrineData.HistoryImageCitations);
        citations.AddRange(shrineData.HistoryCitations);
        citations.AddRange(shrineData.FolkloreImageCitations);
        citations.AddRange(shrineData.FolkloreCitations);
        citations.AddRange(shrineData.GalleryImageCitations);

        return citations
            .GroupBy(c => c.CiteId)
            .Select(g => g.First())
            .OrderBy(c => string.IsNullOrWhiteSpace(c.Title) ? 1 : 0)
            .ThenBy(c => c.Title)
            .ThenBy(c => c.Author)
            .ToList();
    }

    #endregion

    #region CMS Shrine Status 

    public async Task<string> GetShrineStatusByIdCMSAsync(int id, CancellationToken ct)
    {
        var status = await _db.Shrines
            .AsNoTracking()
            .Where(s => s.ShrineId == id)
            .Select(s => s.Status)
            .SingleOrDefaultAsync(ct);

        if (status is null)
            throw new KeyNotFoundException($"Shrine with id {id} not found.");

        return status;
    }

    #endregion

    #region CMS Shrine by HistoryId

    public async Task<int> GetShrineIdByHistoryIdCMSAsync(int historyId, CancellationToken ct)
    {
        var shrineId = await _db.Histories
            .AsNoTracking()
            .Where(h => h.HistoryId == historyId)
            .Select(h => h.ShrineId)
            .SingleOrDefaultAsync(ct);

        if (shrineId == 0)
            throw new KeyNotFoundException($"History with id {historyId} not found.");

        return shrineId;
    }

    #endregion

    #region CMS Shrine by FolkloreId

    public async Task<int> GetShrineIdByFolkloreIdCMSAsync(int folkloreId, CancellationToken ct)
    {
        var shrineId = await _db.Folklores
            .AsNoTracking()
            .Where(f => f.FolkloreId == folkloreId)
            .Select(f => f.ShrineId)
            .SingleOrDefaultAsync(ct);

        if (shrineId == 0)
            throw new KeyNotFoundException($"Folklore with id {folkloreId} not found.");

        return shrineId;
    }

    #endregion

    #region CMS Shrine by ImageId

    public async Task<int> GetShrineIdByImageIdCMSAsync(int imageId, CancellationToken ct)
    {
        var shrineId = await _db.ShrineGalleries
            .AsNoTracking()
            .Where(i => i.ImgId == imageId)
            .Select(i => i.ShrineId)
            .SingleOrDefaultAsync(ct);

        if (shrineId == 0)
            throw new KeyNotFoundException($"Image with id {imageId} not found.");

        return shrineId;
    }

    #endregion


    #region Get Exisiting Import Ids

    public async Task<List<string>> GetExistingImportIdsAsync(List<string> importIds, CancellationToken ct)
    {
        if (importIds == null || importIds.Count == 0)
            return new List<string>();

        return await _db.Shrines
            .AsNoTracking()
            .Where(s => s.InputtedId != null && importIds.Contains(s.InputtedId))
            .Select(s => s.InputtedId!)
            .ToListAsync(ct);
    }

    #endregion

    #region Get Shrine Review History

    public async Task<IReadOnlyList<ShrineReviewDto>> GetShrineReviewHistoryAsync(int shrineId, CancellationToken ct)
    {
        return await _db.ShrineReviews
            .AsNoTracking()
            .Where(s => s.ShrineId == shrineId)
            .OrderByDescending(r => r.SubmittedAt)
            .Select(r => new ShrineReviewDto
            (
                r.ReviewId,
                r.SubmittedAt,
                r.SubmittedBy,
                r.SubmittedByUser.Username,
                r.ReviewedAt,
                r.ReviewedBy,
                r.ReviewedByUser != null ? r.ReviewedByUser.Username : null,
                r.ReviewerComment,
                r.Decision.ToString()
            )).ToListAsync(ct);
    }

    #endregion

    #region Get Shrine Recently Rejected

    public async Task<bool> IsShrineRecentlyRejectedAsync(int shrineId, CancellationToken ct)
    {
        var mostRecentDecision = await _db.ShrineReviews
        .AsNoTracking()
        .Where(r => r.ShrineId == shrineId)
        .OrderByDescending(r => r.SubmittedAt)
        .Select(r => (ReviewDecision?)r.Decision)
        .FirstOrDefaultAsync(ct);

        return mostRecentDecision == ReviewDecision.Rejected;
    }

    #endregion

    #region Get PublicId Image

    public async Task<string?> GetImagePublicIdCMSAsync(int imageId, CancellationToken ct)
    {
        return await _db.Images
            .AsNoTracking()
            .Where(i => i.ImgId == imageId)
            .Select(i => i.PublicId)
            .SingleOrDefaultAsync(ct);
    }

    #endregion

    #region Get PublicId History

    public async Task<string?> GetHistoryImagePublicIdCMSAsync(int historyId, CancellationToken ct)
    {
        return await _db.Histories
            .AsNoTracking()
            .Where(k => k.HistoryId == historyId)
            .Select(k => k.Image!.PublicId)
            .SingleOrDefaultAsync(ct);
    }

    #endregion

    #region Get PublicId Folklore

    public async Task<string?> GetFolkloreImagePublicIdCMSAsync(int folkloreId, CancellationToken ct)
    {
        return await _db.Folklores
            .AsNoTracking()
            .Where(k => k.FolkloreId == folkloreId)
            .Select(k => k.Image!.PublicId)
            .SingleOrDefaultAsync(ct);
    }

    #endregion
}