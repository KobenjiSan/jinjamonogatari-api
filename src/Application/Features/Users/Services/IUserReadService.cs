using Application.Features.Users.Queries.GetUsersList;
using Application.Features.Shrines.Models;
using Application.Features.Users.Models;
using Domain.Entities;

namespace Application.Features.Users.Services;

public interface IUserReadService
{
    Task<bool> EmailExistsAsync(string emailLower, CancellationToken ct);
    Task<bool> UsernameExistsAsync(string username, CancellationToken ct);

    Task<User?> FindByEmailOrUsernameAsync(string identifierLower, CancellationToken ct);
    
    Task<User?> FindByIdAsync(int userId, CancellationToken ct);

    Task<bool> IsShrineInCollectionAsync(int userId, int shrineId, CancellationToken ct);

    Task<IReadOnlyList<ShrinePreviewDto>> GetShrineCollectionCards(
        int userId,
        double? lat,
        double? lon,
        string? q,
        CancellationToken ct
    );

    Task<IReadOnlyList<int>> GetShrineCollectionIds(int userId, CancellationToken ct);

    Task<(IReadOnlyList<UserListDto>, int)> GetUsersListAsync(GetUsersListQuery request, CancellationToken ct);

}
