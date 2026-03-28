using MediatR;

namespace Application.Features.Users.Commands.LoginUserCMS;

public record LoginUserCMSCommand(string Identifier, string Password) : IRequest<LoginUserCMSResult>;

public record LoginUserCMSResult(string AccessToken, string RefreshToken, string Role);

