using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Users.Commands.LogoutUser;

public class LogoutUserHandler : IRequestHandler<LogoutUserCommand, Unit>
{
    private readonly IAppDbContext _db;
    private readonly ITokenService _tokens;

    public LogoutUserHandler(IAppDbContext db, ITokenService tokens)
    {
        _db = db;
        _tokens = tokens;
    }

    public async Task<Unit> Handle(LogoutUserCommand request, CancellationToken ct)
    {
        var raw = request.RefreshToken.Trim();

        var hash = _tokens.HashRefreshToken(raw);

        var stored = await _db.RefreshTokens
            .FirstOrDefaultAsync(x => x.TokenHash == hash, ct);

        if (stored is null)
            return Unit.Value; // idempotent logout

        stored.RevokedAtUtc = DateTime.UtcNow;

        await _db.SaveChangesAsync(ct);

        return Unit.Value;
    }
}