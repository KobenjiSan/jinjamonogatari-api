using Domain.Common;

namespace Domain.Entities;

public class AuditLog : IHasCreatedAt
{
    // PK
    public int AuditId { get; set; }

    // User Data
    public int UserId { get; set; }
    public string Username { get; set; } = string.Empty;

    // Basic Info
    public string Action { get; set; } = string.Empty;
    public string Target { get; set; } = string.Empty;

    // Result
    public bool IsSuccessful { get; set; }
    public string? Message { get; set; }

    // Timestamps
    public DateTime CreatedAt { get; set; }
}