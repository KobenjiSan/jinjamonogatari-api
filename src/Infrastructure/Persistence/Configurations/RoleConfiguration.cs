using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> e)
    {
        // Map entity to table
        e.ToTable("roles");

        // Primary Key
        e.HasKey(x => x.RoleId);
        e.Property(x => x.RoleId).HasColumnName("role_id").ValueGeneratedOnAdd();

        // Role Name
        e.Property(x => x.Name).HasColumnName("name").HasMaxLength(25).IsRequired();
        e.HasIndex(x => x.Name).IsUnique();

        // Seed Initial Roles
        e.HasData(
            new Role { RoleId = 1, Name = "User" },
            new Role { RoleId = 2, Name = "Editor" },
            new Role { RoleId = 3, Name = "Admin" }
        );
    }
}