using Application.Common.Interfaces;
using Application.Features.Users.Services;
using MediatR;

namespace Application.Features.Users.Commands.LogoutUser;

public class LogoutUserHandler : IRequestHandler<LogoutUserCommand, Unit>
{
    private readonly IUserWriteService _writeService;
    private readonly ITokenService _tokens;

    public LogoutUserHandler(IUserWriteService writeService, ITokenService tokens)
    {
        _writeService = writeService;
        _tokens = tokens;
    }

    public async Task<Unit> Handle(LogoutUserCommand request, CancellationToken ct)
    {
        var raw = request.RefreshToken.Trim();
        var hash = _tokens.HashRefreshToken(raw);

        await _writeService.RevokeRefreshTokenAsync(hash, DateTime.UtcNow, ct);

        return Unit.Value; // idempotent logout
    }
}