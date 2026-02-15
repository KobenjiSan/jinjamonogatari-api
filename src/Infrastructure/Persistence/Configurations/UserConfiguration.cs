using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> b)
    {
        b.ToTable("users");

        b.HasKey(x => x.UserId);

        b.Property(x => x.UserId)
            .HasColumnName("user_id")
            .ValueGeneratedOnAdd();

        b.Property(x => x.Email)
            .HasColumnName("email")
            .HasMaxLength(320)
            .IsRequired();

        b.Property(x => x.Username)
            .HasColumnName("username")
            .HasMaxLength(50)
            .IsRequired();

        b.Property(x => x.PassHash)
            .HasColumnName("pass_hash")
            .HasMaxLength(255)
            .IsRequired();

        b.Property(x => x.Phone)
            .HasColumnName("phone")
            .HasMaxLength(30);

        b.Property(x => x.FirstName)
            .HasColumnName("first_name")
            .HasMaxLength(100);

        b.Property(x => x.LastName)
            .HasColumnName("last_name")
            .HasMaxLength(100);

        b.Property(x => x.RoleId)
            .HasColumnName("role_id");

        // timestamps
        b.Property(x => x.CreatedAt)
            .HasColumnName("created_at")
            .HasColumnType("timestamp with time zone")
            .IsRequired();

        b.Property(x => x.UpdatedAt)
            .HasColumnName("updated_at")
            .HasColumnType("timestamp with time zone")
            .IsRequired();

        b.Property(x => x.LastLoginAt)
            .HasColumnName("last_login_at")
            .HasColumnType("timestamp with time zone");

        // indexes + uniqueness
        b.HasIndex(x => x.Email).IsUnique();
        b.HasIndex(x => x.Username).IsUnique();
    }
}