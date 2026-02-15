using Domain.Entities;

namespace Application.Features.Users.Services;

public interface IUserReadService
{
    Task<bool> EmailExistsAsync(string emailLower, CancellationToken ct);
    Task<bool> UsernameExistsAsync(string username, CancellationToken ct);

    Task<User?> FindByEmailOrUsernameAsync(string identifierLower, CancellationToken ct);
    
    Task<User?> FindByIdAsync(int userId, CancellationToken ct);

}
