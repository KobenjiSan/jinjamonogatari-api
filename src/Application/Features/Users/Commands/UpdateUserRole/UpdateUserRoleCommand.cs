using MediatR;

namespace Application.Features.Users.Commands.UpdateUserRole;

// COMMAND
public record UpdateUserRoleCommand(int UserId, string UserRole) : IRequest;