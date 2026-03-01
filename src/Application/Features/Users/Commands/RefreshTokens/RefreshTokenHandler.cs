using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Users.Commands.RefreshTokens;

public class RefreshTokenHandler : IRequestHandler<RefreshTokenCommand, RefreshTokenResult>
{
    private readonly IAppDbContext _db;
    private readonly ITokenService _tokens;
    private readonly ITokenOptions _tokenOptions;

    public RefreshTokenHandler(IAppDbContext db, ITokenService tokens, ITokenOptions tokenOptions)
    {
        _db = db;
        _tokens = tokens;
        _tokenOptions = tokenOptions;
    }

    public async Task<RefreshTokenResult> Handle(RefreshTokenCommand request, CancellationToken ct)
    {
        var raw = request.RefreshToken.Trim();

        // Hash incoming token to match stored hash
        var incomingHash = _tokens.HashRefreshToken(raw);

        // Find stored token row
        var stored = await _db.RefreshTokens
            .FirstOrDefaultAsync(x => x.TokenHash == incomingHash, ct);

        if (stored is null || !stored.IsActive)
            throw new UnauthorizedAccessException("Invalid refresh token.");

        // Load user (we need email for access token claims)
        var user = await _db.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.UserId == stored.UserId, ct);

        if (user is null)
            throw new UnauthorizedAccessException("Invalid refresh token.");

        // Revoke old token (rotation)
        stored.RevokedAtUtc = DateTime.UtcNow;

        // Create new refresh token
        var newRaw = _tokens.CreateRefreshToken();
        var newHash = _tokens.HashRefreshToken(newRaw);

        _db.RefreshTokens.Add(new RefreshToken
        {
            UserId = user.UserId,
            TokenHash = newHash,
            ExpiresAtUtc = DateTime.UtcNow.AddDays(_tokenOptions.RefreshTokenDays)
        });

        await _db.SaveChangesAsync(ct);

        // New access token
        var newAccess = _tokens.CreateAccessToken(user.UserId, user.Email);

        return new RefreshTokenResult(newAccess, newRaw);
    }
}