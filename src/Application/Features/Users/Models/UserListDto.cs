namespace Application.Features.Users.Models;
public record UserListDto(
    // Id
    int UserId,
    // Identity
    string Username,
    string Email,
    // Role
    string RoleName,
    // Created / Updated
    DateTime CreatedAt,
    DateTime UpdatedAt,
    // last Login
    DateTime? LastLoginAt,
    // Saved Shrines
    int SavedShrineCount
);