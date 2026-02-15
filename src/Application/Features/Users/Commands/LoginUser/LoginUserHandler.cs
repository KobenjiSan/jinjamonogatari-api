using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Features.Users.Services;
using MediatR;

namespace Application.Features.Users.Commands.LoginUser;

public class LoginUserHandler : IRequestHandler<LoginUserCommand, LoginUserResult>
{
    private readonly IUserReadService _readService;
    private readonly IUserWriteService _writeService;
    private readonly ITokenService _tokens;
    private readonly IPasswordHasher _passwords;

    public LoginUserHandler(
        IUserReadService readService,
        IUserWriteService writeService,
        ITokenService tokens,
        IPasswordHasher passwords)
    {
        _readService = readService;
        _writeService = writeService;
        _tokens = tokens;
        _passwords = passwords;
    }

    public async Task<LoginUserResult> Handle(LoginUserCommand request, CancellationToken ct)
    {
        var identifier = request.Identifier.Trim().ToLowerInvariant();

        var user = await _readService.FindByEmailOrUsernameAsync(identifier, ct);

        if (user is null)
            throw new NotFoundException("User not found.");

        if (!_passwords.Verify(user.PassHash, request.Password))
            throw new UnauthorizedAccessException("Invalid credentials.");
        
        await _writeService.UpdateLastLoginAsync(user.UserId, ct);

        var accessToken = _tokens.CreateAccessToken(user.UserId, user.Email);

        return new LoginUserResult(accessToken);
    }
}
