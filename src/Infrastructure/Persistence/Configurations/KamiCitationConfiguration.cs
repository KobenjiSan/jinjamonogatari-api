using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class KamiCitationConfiguration : IEntityTypeConfiguration<KamiCitation>
{
    public void Configure(EntityTypeBuilder<KamiCitation> e)
    {
        // Map entity to table
        e.ToTable("kami_citations");

        // Composite primary key
        e.HasKey(x => new { x.KamiId, x.CiteId });

        // Joined Tables
        e.Property(x => x.KamiId).HasColumnName("kami_id").IsRequired();
        e.Property(x => x.CiteId).HasColumnName("cite_id").IsRequired();

        // Timestamp
        e.Property(x => x.CreatedAt).HasColumnName("created_at").HasColumnType("timestamp with time zone").IsRequired();

        // Relationships
        //FK: kami_citations.kami_id -> kami.kami_id
        e.HasOne(x => x.Kami)                       // Each KamiCitation references ONE Kami
            .WithMany(x => x.KamiCitations)         // A Kami can have MANY KamiCitation links
            .HasForeignKey(x => x.KamiId)           // FK column stored on kami_citations
            .OnDelete(DeleteBehavior.Cascade);      // Deleting a Kami removes its citation links

        //FK: kami_citations.cite_id -> citations.cite_id
        e.HasOne(x => x.Citation)                   // Each KamiCitation references ONE Citation
            .WithMany(x => x.KamiCitations)         // A Citation can have MANY KamiCitation links
            .HasForeignKey(x => x.CiteId)           // FK column stored on kami_citations
            .OnDelete(DeleteBehavior.Cascade);      // Deleting a Citation removes its kami links
    }
}