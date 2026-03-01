using MediatR;

namespace Application.Features.Users.Commands.LogoutUser;

public record LogoutUserCommand(string RefreshToken) : IRequest<Unit>;