using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

namespace Infrastructure.Persistence.Configurations;

public class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>
{
    public void Configure(EntityTypeBuilder<AuditLog> e)
    {
        // Map entity to table
        e.ToTable("audit_log");

        // Primary key
        e.HasKey(x => x.AuditId);
        e.Property(x => x.AuditId).HasColumnName("audit_id").ValueGeneratedOnAdd();

        // User Data
        e.Property(x => x.UserId).HasColumnName("user_id").IsRequired();
        e.Property(x => x.Username).HasColumnName("username").IsRequired();

        // Basic Info
        e.Property(x => x.Action).HasColumnName("action").IsRequired();
        e.Property(x => x.Target).HasColumnName("target").IsRequired();

        // Result
        e.Property(x => x.IsSuccessful).HasColumnName("is_successful").IsRequired();
        e.Property(x => x.Message).HasColumnName("message");

        // Timestamps
        e.Property(x => x.CreatedAt).HasColumnName("created_at").HasColumnType("timestamp with time zone").IsRequired();
    }
}