using Domain.Entities;

namespace Application.Features.Users.Services;

public interface IUserWriteService
{
    Task<User> CreateUserAsync(
        string email,
        string username,
        string passwordHash,
        CancellationToken ct);

    Task UpdateLastLoginAsync(int userId, CancellationToken ct);

    Task AddShrineToCollectionAsync(int userId, int shrineId, CancellationToken ct);

    Task RemoveShrineFromCollectionAsync(int userId, int shrineId, CancellationToken ct);

    Task AddRefreshTokenAsync(int userId, string tokenHash, DateTime expiresAtUtc, CancellationToken ct);

    Task RevokeRefreshTokenAsync(string tokenHash, DateTime revokedAtUtc, CancellationToken ct);

    Task<(int UserId, string Email)> RotateRefreshTokenAsync(
        string incomingTokenHash,
        string newTokenHash,
        DateTime newExpiresAtUtc,
        DateTime revokedAtUtc,
        CancellationToken ct);

    Task<User> UpdateMyProfileAsync(
        int userId,
        bool hasFirstName, string? firstName,
        bool hasLastName, string? lastName,
        bool hasPhone, string? phone,
        CancellationToken ct);
}