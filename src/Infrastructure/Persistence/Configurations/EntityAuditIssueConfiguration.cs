using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

namespace Infrastructure.Persistence.Configurations;

public class EntityAuditIssueConfiguration : IEntityTypeConfiguration<EntityAuditIssue>
{
    public void Configure(EntityTypeBuilder<EntityAuditIssue> e)
    {
        // Map entity to table
        e.ToTable("entity_audit_issue");

        // Primary key
        e.HasKey(x => x.EntityAuditIssueId);
        e.Property(x => x.EntityAuditIssueId).HasColumnName("entity_audit_issue_id").ValueGeneratedOnAdd();

        // Audit foreign key
        e.Property(x => x.EntityAuditId).HasColumnName("entity_audit_id");

        // Content
        e.Property(x => x.Severity).HasColumnName("severity");
        e.Property(x => x.Field).HasColumnName("field");
        e.Property(x => x.Message).HasColumnName("message");

        e.Property(x => x.RelatedItemType).HasColumnName("related_item_type");
        e.Property(x => x.RelatedItemId).HasColumnName("related_item_id");

        // Timestamps
        e.Property(x => x.CreatedAt).HasColumnName("created_at").HasColumnType("timestamp with time zone").IsRequired();
    }
}