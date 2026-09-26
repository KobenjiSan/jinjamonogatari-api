using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

namespace Infrastructure.Persistence.Configurations;

public class EntityAuditConfiguration : IEntityTypeConfiguration<EntityAudit>
{
    public void Configure(EntityTypeBuilder<EntityAudit> e)
    {
        // Map entity to table
        e.ToTable("entity_audit");

        // Primary key
        e.HasKey(x => x.EntityAuditId);
        e.Property(x => x.EntityAuditId).HasColumnName("entity_audit_id").ValueGeneratedOnAdd();

        // Content
        e.Property(x => x.ErrorCount).HasColumnName("error_count");
        e.Property(x => x.WarningCount).HasColumnName("warning_count");
        e.Property(x => x.CanSubmit).HasColumnName("can_submit");

        // Timestamps
        e.Property(x => x.CreatedAt).HasColumnName("created_at").HasColumnType("timestamp with time zone").IsRequired();
        e.Property(x => x.UpdatedAt).HasColumnName("updated_at").HasColumnType("timestamp with time zone").IsRequired();
    
        // Relationship config:
        e.HasMany(x => x.Issues)                      // Audit has many issues
            .WithOne(i => i.EntityAudit)              // issue links to one audit
            .HasForeignKey(i => i.EntityAuditId)      // link to FK
            .OnDelete(DeleteBehavior.Cascade);        // Delete issues with audit
    }
}