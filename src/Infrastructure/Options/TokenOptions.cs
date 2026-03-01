using Application.Common.Interfaces;
using Microsoft.Extensions.Options;

namespace Infrastructure.Options;

public class TokenOptions : ITokenOptions
{
    private readonly JwtSettings _jwt;

    public TokenOptions(IOptions<JwtSettings> jwtOptions)
    {
        _jwt = jwtOptions.Value;
    }

    public int AccessTokenMinutes => _jwt.AccessTokenMinutes;
    public int RefreshTokenDays => _jwt.RefreshTokenDays;
}