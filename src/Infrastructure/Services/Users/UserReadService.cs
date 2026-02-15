using Application.Features.Users.Services;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.Users;

public class UserReadService : IUserReadService
{
    private readonly AppDbContext _db;

    public UserReadService(AppDbContext db)
    {
        _db = db;
    }

    public Task<bool> EmailExistsAsync(string emailLower, CancellationToken ct)
        => _db.Users.AsNoTracking().AnyAsync(u => u.Email == emailLower, ct);

    public Task<bool> UsernameExistsAsync(string username, CancellationToken ct)
        => _db.Users.AsNoTracking().AnyAsync(u => u.Username == username, ct);

    public Task<User?> FindByEmailOrUsernameAsync(string identifierLower, CancellationToken ct)
        => _db.Users.AsNoTracking()
            .FirstOrDefaultAsync(u =>
                u.Email.ToLower() == identifierLower || u.Username.ToLower() == identifierLower, ct);

    public Task<User?> FindByIdAsync(int userId, CancellationToken ct)
        => _db.Users.AsNoTracking()
            .FirstOrDefaultAsync(u => u.UserId == userId, ct);
}
