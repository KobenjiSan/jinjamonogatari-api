namespace Infrastructure.Persistence.Entities;

public class RefreshToken
{
    public Guid Id { get; set; } = Guid.NewGuid();

    // link to user later
    public string UserEmail { get; set; } = default!;

    public string TokenHash { get; set; } = default!;

    public DateTime ExpiresAtUtc { get; set; }
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

    public DateTime? RevokedAtUtc { get; set; }

    public bool IsExpired => DateTime.UtcNow >= ExpiresAtUtc;
    public bool IsRevoked => RevokedAtUtc != null;
    public bool IsActive => !IsExpired && !IsRevoked;
}