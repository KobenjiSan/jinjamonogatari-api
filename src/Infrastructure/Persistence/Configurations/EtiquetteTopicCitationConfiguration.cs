using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

namespace Infrastructure.Persistence.Configurations;

public class EtiquetteTopicCitationConfiguration : IEntityTypeConfiguration<EtiquetteTopicCitation>
{
    public void Configure(EntityTypeBuilder<EtiquetteTopicCitation> e)
    {
        e.ToTable("etiquette_citations");

        e.Property(x => x.TopicId).HasColumnName("topic_id");
        e.Property(x => x.CiteId).HasColumnName("cite_id");
        e.Property(x => x.CreatedAt).HasColumnName("created_at").HasColumnType("timestamp with time zone");

        e.HasKey(x => new { x.TopicId, x.CiteId });

        e.HasOne(x => x.Topic)
         .WithMany(t => t.TopicCitations)
         .HasForeignKey(x => x.TopicId)
         .OnDelete(DeleteBehavior.Cascade);

        e.HasOne(x => x.Citation)
         .WithMany(c => c.EtiquetteTopicCitations)
         .HasForeignKey(x => x.CiteId)
         .OnDelete(DeleteBehavior.Cascade);
    }
}