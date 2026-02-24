using Application.Features.Shrines.Models;
using Application.Features.Users.Services;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

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

    public async Task<IReadOnlyList<ShrinePreviewDto>> GetShrineCollectionCards(int userId, CancellationToken ct)
    {
        return await _db.UserCollections
            .AsNoTracking()
            .Where(uc => uc.UserId == userId)
            .OrderByDescending(uc => uc.CreatedAt)
            .Where(uc => uc.Shrine.PublishedAt != null && uc.Shrine.Slug != null)
            .Select(uc => new ShrinePreviewDto(
                uc.Shrine.ShrineId,
                uc.Shrine.Slug!,
                uc.Shrine.NameEn,
                uc.Shrine.NameJp,
                uc.Shrine.Image != null ? uc.Shrine.Image.ImgSource : null,
                uc.Shrine.ShrineDesc,
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
