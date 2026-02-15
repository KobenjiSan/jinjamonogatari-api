using Application.Features.Users.Services;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.Users;

public class UserWriteService : IUserWriteService
{
    private readonly AppDbContext _db;

    public UserWriteService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<User> CreateUserAsync(
        string email,
        string username,
        string passwordHash,
        CancellationToken ct)
    {
        var now = DateTime.UtcNow;

        var user = new User
        {
            Email = email,
            Username = username,
            PassHash = passwordHash,
            CreatedAt = now,
            UpdatedAt = now
        };

        _db.Users.Add(user);
        await _db.SaveChangesAsync(ct);

        return user;
    }

    public async Task UpdateLastLoginAsync(int userId, CancellationToken ct)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.UserId == userId, ct);
        if (user is null) return;

        user.LastLoginAt = DateTime.UtcNow;
        user.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync(ct);
    }
}
