using MediatR;

namespace Application.Features.Users.Commands.LoginUser;

public record LoginUserCommand(string Identifier, string Password) : IRequest<LoginUserResult>;

public record LoginUserResult(string AccessToken);

