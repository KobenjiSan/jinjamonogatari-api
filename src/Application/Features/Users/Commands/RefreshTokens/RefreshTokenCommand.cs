using MediatR;

namespace Application.Features.Users.Commands.RefreshTokens;

public record RefreshTokenCommand(string RefreshToken) : IRequest<RefreshTokenResult>;

public record RefreshTokenResult(string AccessToken, string RefreshToken);