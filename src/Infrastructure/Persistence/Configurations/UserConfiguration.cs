using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> e)
    {
        // Map entity to table
        e.ToTable("users");

        // Primary key
        e.HasKey(x => x.UserId);
        e.Property(x => x.UserId).HasColumnName("user_id").ValueGeneratedOnAdd();

        // Required for signup / login
        e.Property(x => x.Email).HasColumnName("email").HasMaxLength(320).IsRequired();
        e.HasIndex(x => x.Email).IsUnique();
        e.Property(x => x.Username).HasColumnName("username").HasMaxLength(50).IsRequired();
        e.HasIndex(x => x.Username).IsUnique();
        e.Property(x => x.PassHash).HasColumnName("pass_hash").HasMaxLength(255).IsRequired();

        // Optional profile fields (editable later)
        e.Property(x => x.Phone).HasColumnName("phone").HasMaxLength(30);
        e.Property(x => x.FirstName).HasColumnName("first_name").HasMaxLength(100);
        e.Property(x => x.LastName).HasColumnName("last_name").HasMaxLength(100);

        // TODO: not implemented yet
        e.Property(x => x.RoleId).HasColumnName("role_id");

        // timestamps
        e.Property(x => x.CreatedAt).HasColumnName("created_at").HasColumnType("timestamp with time zone").IsRequired();
        e.Property(x => x.UpdatedAt).HasColumnName("updated_at").HasColumnType("timestamp with time zone").IsRequired();
        e.Property(x => x.LastLoginAt).HasColumnName("last_login_at").HasColumnType("timestamp with time zone");

        // Relationship config:
    }
}