using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class KamiConfiguration : IEntityTypeConfiguration<Kami>
{
    public void Configure(EntityTypeBuilder<Kami> e)
    {
        // Map entity to table
        e.ToTable("kami");

        // Primary key
        e.HasKey(x => x.KamiId);
        e.Property(x => x.KamiId).HasColumnName("kami_id").ValueGeneratedOnAdd();

        // Content
        e.Property(x => x.NameEn).HasColumnName("name_en");
        e.Property(x => x.NameJp).HasColumnName("name_jp");
        e.Property(x => x.Desc).HasColumnName("desc");

        // Image
        e.Property(x => x.ImgId).HasColumnName("img_id");

        // Audit
        e.Property(x => x.EntityAuditId).HasColumnName("entity_audit_id");

        // Publishing state
        e.Property(x => x.Status).HasColumnName("status").HasConversion<string>(); // .HasConversion<string>() converts C# enum to string in db

        // Timestamps 
        e.Property(x => x.PublishedAt).HasColumnName("published_at").HasColumnType("timestamp with time zone");
        e.Property(x => x.CreatedAt).HasColumnName("created_at").HasColumnType("timestamp with time zone").IsRequired();
        e.Property(x => x.UpdatedAt).HasColumnName("updated_at").HasColumnType("timestamp with time zone").IsRequired();

        // Relationship config:

        e.HasOne(x => x.Image)                          // kami has one image
            .WithMany()                                 // image can link to many kami
            .HasForeignKey(x => x.ImgId)                // link to our FK
            .OnDelete(DeleteBehavior.SetNull);          // if images is deleted, set ImgId / Image to null

        e.HasOne(x => x.EntityAudit)                    // kami has one EntityAudit
            .WithOne()                                  // EntityAudit belongs to one Kami
            .HasForeignKey<Kami>(x => x.EntityAuditId)  // foreign key is stored on the Kami table in EntityAuditId
            .OnDelete(DeleteBehavior.Restrict);         // prevents EntityAudit from being deleted while Kami still references it.
    }
}