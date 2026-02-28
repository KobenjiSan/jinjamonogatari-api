using Application.Features.Shrines.Models;
using Application.Features.Users.Services;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;

namespace Infrastructure.Services.Users;

public class UserReadService : IUserReadService
{
    private readonly AppDbContext _db;

    public UserReadService(AppDbContext db)
    {
        _db = db;
    }

    public Task<bool> EmailExistsAsync(string emailLower, CancellationToken ct)
        => _db.Users.AsNoTracking().AnyAsync(u => u.Email == emailLower, ct);

    public Task<bool> UsernameExistsAsync(string username, CancellationToken ct)
        => _db.Users.AsNoTracking().AnyAsync(u => u.Username == username, ct);

    public Task<User?> FindByEmailOrUsernameAsync(string identifierLower, CancellationToken ct)
        => _db.Users.AsNoTracking()
            .FirstOrDefaultAsync(u =>
                u.Email.ToLower() == identifierLower || u.Username.ToLower() == identifierLower, ct);

    public Task<User?> FindByIdAsync(int userId, CancellationToken ct)
        => _db.Users.AsNoTracking()
            .FirstOrDefaultAsync(u => u.UserId == userId, ct);

    public Task<bool> IsShrineInCollectionAsync(int userId, int shrineId, CancellationToken ct)
        => _db.UserCollections
            .AsNoTracking()
            .AnyAsync(x => x.UserId == userId && x.ShrineId == shrineId, ct);

    public async Task<IReadOnlyList<ShrinePreviewDto>> GetShrineCollectionCards(
        int userId,
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

        return await _db.UserCollections
            .AsNoTracking()
            .Where(uc => uc.UserId == userId)
            .OrderByDescending(uc => uc.CreatedAt)
            .Where(uc => uc.Shrine.PublishedAt != null && uc.Shrine.Slug != null)
            .Where(uc => !hasQ || (
                EF.Functions.ILike(uc.Shrine.NameEn ?? "", pattern!) ||
                EF.Functions.ILike(uc.Shrine.NameJp ?? "", pattern!) ||
                EF.Functions.ILike(uc.Shrine.Slug ?? "", pattern!) ||
                EF.Functions.ILike(uc.Shrine.ShrineDesc ?? "", pattern!) ||
                EF.Functions.ILike(uc.Shrine.Prefecture ?? "", pattern!) ||
                EF.Functions.ILike(uc.Shrine.City ?? "", pattern!) ||
                EF.Functions.ILike(uc.Shrine.Ward ?? "", pattern!) ||
                EF.Functions.ILike(uc.Shrine.Locality ?? "", pattern!) ||
                EF.Functions.ILike(uc.Shrine.AddressRaw ?? "", pattern!) ||
                EF.Functions.ILike(uc.Shrine.PostalCode ?? "", pattern!) ||
                EF.Functions.ILike(uc.Shrine.Country ?? "", pattern!) ||
                uc.Shrine.ShrineTags.Any(st =>
                    EF.Functions.ILike(st.Tag.TitleEn ?? "", pattern!) ||
                    EF.Functions.ILike(st.Tag.TitleJp ?? "", pattern!)
                )
            ))
            .Select(uc => new ShrinePreviewDto(
                uc.Shrine.ShrineId,
                uc.Shrine.Slug!,
                uc.Shrine.Lat.HasValue ? (double?)uc.Shrine.Lat.Value : null,
                uc.Shrine.Lon.HasValue ? (double?)uc.Shrine.Lon.Value : null,
                uc.Shrine.NameEn,
                uc.Shrine.NameJp,
                uc.Shrine.Image != null ? uc.Shrine.Image.ImgSource : null,
                uc.Shrine.ShrineDesc,
                (hasUserPoint && uc.Shrine.Location != null)
                    ? EF.Functions.Distance(uc.Shrine.Location!, userPoint!, true)
                    : (double?)null,
                uc.Shrine.ShrineTags
                    .Select(st => new TagDto(
                        st.TagId,
                        st.Tag.TitleEn,
                        st.Tag.TitleJp
                    )).ToList()
            )).ToListAsync(ct);
    }

    public async Task<IReadOnlyList<int>> GetShrineCollectionIds(int userId, CancellationToken ct)
    {
        return await _db.UserCollections
            .AsNoTracking()
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.CreatedAt)
            .Select(x => x.ShrineId)
            .ToListAsync(ct);
    }
}
