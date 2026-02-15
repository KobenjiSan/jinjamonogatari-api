using MediatR;

namespace Application.Features.Users.Commands.RegisterUser;

public record RegisterUserCommand(string Email, string Username, string Password) : IRequest;
