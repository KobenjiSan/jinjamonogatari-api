using Application.Common.Interfaces;
using Application.Features.Users.Services;
using MediatR;

namespace Application.Features.Users.Commands.RefreshTokens;

public class RefreshTokenHandler : IRequestHandler<RefreshTokenCommand, RefreshTokenResult>
{
    private readonly IUserWriteService _writeService;
    private readonly ITokenService _tokens;
    private readonly ITokenOptions _tokenOptions;

    public RefreshTokenHandler(
        IUserWriteService writeService,
        ITokenService tokens,
        ITokenOptions tokenOptions)
    {
        _writeService = writeService;
        _tokens = tokens;
        _tokenOptions = tokenOptions;
    }

    public async Task<RefreshTokenResult> Handle(RefreshTokenCommand request, CancellationToken ct)
    {
        var raw = request.RefreshToken.Trim();

        // Hash incoming token to match stored hash
        var incomingHash = _tokens.HashRefreshToken(raw);

        // Create new refresh token (rotation)
        var newRaw = _tokens.CreateRefreshToken();
        var newHash = _tokens.HashRefreshToken(newRaw);

        var (userId, email) = await _writeService.RotateRefreshTokenAsync(
            incomingHash,
            newHash,
            DateTime.UtcNow.AddDays(_tokenOptions.RefreshTokenDays),
            DateTime.UtcNow,
            ct);

        // New access token
        var newAccess = _tokens.CreateAccessToken(userId, email);

        return new RefreshTokenResult(newAccess, newRaw);
    }
}