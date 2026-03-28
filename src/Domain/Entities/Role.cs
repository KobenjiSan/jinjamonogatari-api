namespace Domain.Entities;

public class Role
{
    public int RoleId { get; set; }   // PK
    public string Name { get; set; } = default!; // "User", "Editor", "Admin"

    // Navigation
    public ICollection<User> Users { get; set; } = new List<User>();
}