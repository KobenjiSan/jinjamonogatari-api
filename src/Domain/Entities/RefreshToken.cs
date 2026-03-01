using Domain.Common;

namespace Domain.Entities;

public class RefreshToken
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public int UserId { get; set; }
    public User? User { get; set; }

    public string TokenHash { get; set; } = default!;

    public DateTime ExpiresAtUtc { get; set; }
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

    // Rotation/revocation
    public DateTime? RevokedAtUtc { get; set; }

    public bool IsExpired => DateTime.UtcNow >= ExpiresAtUtc;
    public bool IsRevoked => RevokedAtUtc != null;
    public bool IsActive => !IsExpired && !IsRevoked;
}