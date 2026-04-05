using Application.Common.Exceptions;
using Application.Features.Users.Services;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.Users;

public class UserWriteService : IUserWriteService
{
    private readonly AppDbContext _db;

    public UserWriteService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<User> CreateUserAsync(
        string email,
        string username,
        string passwordHash,
        CancellationToken ct)
    {
        var now = DateTime.UtcNow;

        var user = new User
        {
            Email = email,
            Username = username,
            PassHash = passwordHash,
            CreatedAt = now,
            UpdatedAt = now
        };

        _db.Users.Add(user);
        await _db.SaveChangesAsync(ct);

        return user;
    }

    public async Task UpdateLastLoginAsync(int userId, CancellationToken ct)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.UserId == userId, ct);
        if (user is null) return;

        user.LastLoginAt = DateTime.UtcNow;
        user.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync(ct);
    }

    public async Task AddShrineToCollectionAsync(int userId, int shrineId, CancellationToken ct)
    {
        // idempotent save (if already saved, do nothing)
        var alreadySaved = await _db.UserCollections
            .AnyAsync(x => x.UserId == userId && x.ShrineId == shrineId, ct);

        if (alreadySaved) return;

        _db.UserCollections.Add(new UserCollection
        {
            UserId = userId,
            ShrineId = shrineId
        });

        await _db.SaveChangesAsync(ct);
    }

    public async Task RemoveShrineFromCollectionAsync(int userId, int shrineId, CancellationToken ct)
    {
        var entry = await _db.UserCollections
            .FirstOrDefaultAsync(x => x.UserId == userId && x.ShrineId == shrineId, ct);

        if (entry is null) return; // idempotent delete

        _db.UserCollections.Remove(entry);
        await _db.SaveChangesAsync(ct);
    }

    public async Task AddRefreshTokenAsync(int userId, string tokenHash, DateTime expiresAtUtc, CancellationToken ct)
    {
        _db.RefreshTokens.Add(new RefreshToken
        {
            UserId = userId,
            TokenHash = tokenHash,
            ExpiresAtUtc = expiresAtUtc
        });

        await _db.SaveChangesAsync(ct);
    }

    public async Task RevokeRefreshTokenAsync(string tokenHash, DateTime revokedAtUtc, CancellationToken ct)
    {
        var stored = await _db.RefreshTokens
            .FirstOrDefaultAsync(x => x.TokenHash == tokenHash, ct);

        if (stored is null)
            return; // idempotent

        if (stored.RevokedAtUtc is not null)
            return; // idempotent

        stored.RevokedAtUtc = revokedAtUtc;

        await _db.SaveChangesAsync(ct);
    }

    public async Task<(int UserId, string Email, string Role)> RotateRefreshTokenAsync(
        string incomingTokenHash,
        string newTokenHash,
        DateTime newExpiresAtUtc,
        DateTime revokedAtUtc,
        CancellationToken ct)
    {
        var stored = await _db.RefreshTokens
            .FirstOrDefaultAsync(x => x.TokenHash == incomingTokenHash, ct);

        if (stored is null || !stored.IsActive)
            throw new UnauthorizedAccessException("Invalid refresh token.");

        // Load user (email needed for access token claims)
        var user = await _db.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.UserId == stored.UserId, ct);

        if (user is null)
            throw new UnauthorizedAccessException("Invalid refresh token.");

        // Revoke old token (rotation)
        stored.RevokedAtUtc = revokedAtUtc;

        // Add new token
        _db.RefreshTokens.Add(new RefreshToken
        {
            UserId = user.UserId,
            TokenHash = newTokenHash,
            ExpiresAtUtc = newExpiresAtUtc
        });

        await _db.SaveChangesAsync(ct);

        return (user.UserId, user.Email, user.Role!.Name);
    }

    public async Task<User> UpdateMyProfileAsync(
        int userId,
        bool hasFirstName, string? firstName,
        bool hasLastName, string? lastName,
        bool hasPhone, string? phone,
        CancellationToken ct)
    {
        var user = await _db.Users
            .FirstOrDefaultAsync(u => u.UserId == userId, ct);

        if (user is null)
            throw new ArgumentException("User not found.");

        if (hasFirstName)
            user.FirstName = firstName; // can be null to clear

        if (hasLastName)
            user.LastName = lastName; // can be null to clear

        if (hasPhone)
            user.Phone = phone; // can be null to clear
        user.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync(ct);

        return user;
    }

    // Note: This will not work for editors / admins linked to ShrineReviews. (Fix later?)
    public async Task DeleteUserAsync(int userId, CancellationToken ct)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.UserId == userId, ct);
        if (user is null) throw new NotFoundException($"User {userId} was not found.");

        _db.Users.Remove(user);
        await _db.SaveChangesAsync(ct);
    }

    public async Task UpdateUserRoleAsync(int userId, string userRole, CancellationToken ct)
    {
        // Get & Validate User
        var user = await _db.Users.FirstOrDefaultAsync(u => u.UserId == userId, ct);
        if (user is null) throw new ArgumentException("User not found.");

        // Get & Validate Role
        var role = await _db.Roles.FirstOrDefaultAsync(r => r.Name == userRole, ct);
        if (role is null) throw new BadRequestException($"Role '{userRole}' was not found.");

        // Update Role
        user.RoleId = role.RoleId;
        user.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync(ct);
    }
}