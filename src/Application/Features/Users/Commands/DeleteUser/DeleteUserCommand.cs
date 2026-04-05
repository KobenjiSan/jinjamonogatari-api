using MediatR;

namespace Application.Features.Users.Commands.DeleteUser;

// COMMAND
public record DeleteUserCommand(int UserId) : IRequest;