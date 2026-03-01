using Domain.Common;

namespace Domain.Entities;

public class User : IHasTimestamps
{
    // PK
    public int UserId { get; set; }
    
    // Required for signup / login
    public string Email { get; set; } = default!;
    public string Username { get; set; } = default!;
    public string PassHash { get; set; } = default!;

    // Optional profile fields (editable later)
    public string? Phone { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }

    // TODO: not implemented yet
    public int? RoleId { get; set; }

    // Timestamps
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? LastLoginAt { get; set; }

    // Relationships (collections)
    public ICollection<UserCollection> UserCollections { get; set; } = new List<UserCollection>();
    public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
}