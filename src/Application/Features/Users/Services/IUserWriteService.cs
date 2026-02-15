using Domain.Entities;

namespace Application.Features.Users.Services;

public interface IUserWriteService
{
    Task<User> CreateUserAsync(
        string email,
        string username,
        string passwordHash,
        CancellationToken ct);

    Task UpdateLastLoginAsync(int userId, CancellationToken ct);

}
