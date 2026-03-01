using Application.Common.Interfaces;
using Application.Features.Users.Services;
using Domain.Entities;
using MediatR;

namespace Application.Features.Users.Commands.RegisterUser;

public class RegisterUserHandler : IRequestHandler<RegisterUserCommand, RegisterUserResult>
{
    private readonly IUserReadService _readService;
    private readonly IUserWriteService _writeService;
    private readonly IPasswordHasher _passwords;
    private readonly ITokenService _tokens;
    private readonly IAppDbContext _db;
    private readonly ITokenOptions _tokenOptions;

     public RegisterUserHandler(
        IUserReadService readService,
        IUserWriteService writeService,
        IPasswordHasher passwords,
        ITokenService tokens,
        IAppDbContext db,
        ITokenOptions tokenOptions)
    {
        _readService = readService;
        _writeService = writeService;
        _passwords = passwords;
        _tokens = tokens;
        _db = db;
        _tokenOptions = tokenOptions;
    }

    public async Task<RegisterUserResult> Handle(RegisterUserCommand request, CancellationToken ct)
    {
        var email = request.Email.Trim().ToLowerInvariant();
        var username = request.Username.Trim();

        // uniqueness checks
        if (await _readService.EmailExistsAsync(email, ct))
            throw new ArgumentException("Email already in use.");

        if (await _readService.UsernameExistsAsync(username, ct))
            throw new ArgumentException("Username already in use.");

        var passHash = _passwords.Hash(request.Password);

        var user = await _writeService.CreateUserAsync(email, username, passHash, ct);

        var accessToken = _tokens.CreateAccessToken(user.UserId, user.Email);

        // Refresh token issuance
        var rawRefresh = _tokens.CreateRefreshToken();
        var refreshHash = _tokens.HashRefreshToken(rawRefresh);

        _db.RefreshTokens.Add(new RefreshToken
        {
            UserId = user.UserId,
            TokenHash = refreshHash,
            ExpiresAtUtc = DateTime.UtcNow.AddDays(_tokenOptions.RefreshTokenDays)
        });

        await _db.SaveChangesAsync(ct);

        return new RegisterUserResult(accessToken, rawRefresh);
    }
}
