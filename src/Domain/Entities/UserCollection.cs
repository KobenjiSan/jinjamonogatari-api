using Domain.Common;

namespace Domain.Entities;

public class UserCollection : IHasCreatedAt
{
    // Connected Tables
    public int UserId { get; set; }
    public User User { get; set; } = null!;

    public int ShrineId { get; set; }
    public Shrine Shrine { get; set; } = null!;

    // Timestamps
    public DateTime CreatedAt { get; set; }
}