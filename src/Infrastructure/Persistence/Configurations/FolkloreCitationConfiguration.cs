using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class FolkloreCitationConfiguration : IEntityTypeConfiguration<FolkloreCitation>
{
    public void Configure(EntityTypeBuilder<FolkloreCitation> e)
    {
        // Map entity to table
        e.ToTable("folklore_citations");

        // Composite primary key
        e.HasKey(x => new { x.FolkloreId, x.CiteId });

        // Joined Tables
        e.Property(x => x.FolkloreId).HasColumnName("folklore_id").IsRequired();
        e.Property(x => x.CiteId).HasColumnName("cite_id").IsRequired();

        // Timestamp
        e.Property(x => x.CreatedAt).HasColumnName("created_at").HasColumnType("timestamp with time zone").IsRequired();

        // Relationship config:
        //FK: folklore_citations.folklore_id -> folklore.folklore_id
        e.HasOne(x => x.Folklore)
            .WithMany(x => x.FolkloreCitations)
            .HasForeignKey(x => x.FolkloreId)
            .OnDelete(DeleteBehavior.Cascade);

        //FK: folklore_citations.cite_id -> citations.cite_id
        e.HasOne(x => x.Citation)
            .WithMany(x => x.FolkloreCitations)
            .HasForeignKey(x => x.CiteId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}