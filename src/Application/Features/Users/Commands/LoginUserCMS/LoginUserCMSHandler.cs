using Application.Common.Exceptions;
using Application.Features.Users.Services;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Features.Users.Commands.LoginUserCMS;

public class LoginUserCMSHandler : IRequestHandler<LoginUserCMSCommand, LoginUserCMSResult>
{
    private readonly IUserReadService _readService;
    private readonly IUserWriteService _writeService;
    private readonly ITokenService _tokens;
    private readonly IPasswordHasher _passwords;
    private readonly ITokenOptions _tokenOptions;

    public LoginUserCMSHandler(
        IUserReadService readService,
        IUserWriteService writeService,
        ITokenService tokens,
        IPasswordHasher passwords,
        ITokenOptions tokenOptions)
    {
        _readService = readService;
        _writeService = writeService;
        _tokens = tokens;
        _passwords = passwords;
        _tokenOptions = tokenOptions;
    }

    public async Task<LoginUserCMSResult> Handle(LoginUserCMSCommand request, CancellationToken ct)
    {
        var identifier = request.Identifier.Trim().ToLowerInvariant();

        var user = await _readService.FindByEmailOrUsernameAsync(identifier, ct);

        if (user is null)
            throw new NotFoundException("User not found.");

        if (!_passwords.Verify(user.PassHash, request.Password))
            throw new UnauthorizedAccessException("Invalid credentials.");

        if (user.Role is null)
            throw new UnauthorizedAccessException("User role not found.");

        if (user.Role?.Name is not ("Admin" or "Editor"))
            throw new UnauthorizedAccessException("Unauthorized User.");

        await _writeService.UpdateLastLoginAsync(user.UserId, ct);

        var accessToken = _tokens.CreateAccessToken(user.UserId, user.Email, user.Role!.Name);

        // Refresh token issuance
        var rawRefresh = _tokens.CreateRefreshToken();
        var refreshHash = _tokens.HashRefreshToken(rawRefresh);

        await _writeService.AddRefreshTokenAsync(
            user.UserId,
            refreshHash,
            DateTime.UtcNow.AddDays(_tokenOptions.RefreshTokenDays),
            ct);

        return new LoginUserCMSResult(accessToken, rawRefresh, user.Role!.Name);
    }
}