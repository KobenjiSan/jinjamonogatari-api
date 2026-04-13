using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Application.Common.Interfaces;
using Infrastructure.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Services.Auth;

public class TokenService : ITokenService
{
    private readonly JwtSettings _jwt;

    public TokenService(IOptions<JwtSettings> jwtOptions)
    {
        _jwt = jwtOptions.Value;
    }

    public string CreateAccessToken(int userId, string email, string role)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new(JwtRegisteredClaimNames.Email, email),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // Token Instance (ID)
            new(ClaimTypes.Role, role),
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256); // Scrambles(SHA) token using secret(HMAC) - Secure Hash Algorithm / Hash-based Message Authentication Code

        var token = new JwtSecurityToken(
            issuer: _jwt.Issuer,
            audience: _jwt.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwt.AccessTokenMinutes),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    // =========================
    // Refresh token helpers
    // =========================

    // Returns the RAW refresh token string (send to client once).
    public string CreateRefreshToken()
    {
        // 32 bytes => 256 bits of entropy
        var bytes = RandomNumberGenerator.GetBytes(32);
        return Convert.ToBase64String(bytes);
    }

    // Returns a stable hash for DB lookup/storage.
    // NOTE: In production this should add a dedicated pepper from env/config
    // For now reuse Jwt.Key as pepper
    public string HashRefreshToken(string rawToken)
    {
        var peppered = $"{rawToken}:{_jwt.Key}";
        var bytes = Encoding.UTF8.GetBytes(peppered); // Computers hash bytes not strings
        var hash = SHA256.HashData(bytes);
        return Convert.ToBase64String(hash);
    }
}