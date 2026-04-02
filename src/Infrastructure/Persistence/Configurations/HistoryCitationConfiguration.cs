using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class HistoryCitationConfiguration : IEntityTypeConfiguration<HistoryCitation>
{
    public void Configure(EntityTypeBuilder<HistoryCitation> e)
    {
        // Map entity to table
        e.ToTable("history_citations");

        // Composite primary key
        e.HasKey(x => new { x.HistoryId, x.CiteId });

        // Joined Tables
        e.Property(x => x.HistoryId).HasColumnName("history_id").IsRequired();
        e.Property(x => x.CiteId).HasColumnName("cite_id").IsRequired();

        // Timestamp
        e.Property(x => x.CreatedAt).HasColumnName("created_at").HasColumnType("timestamp with time zone").IsRequired();

        // Relationship config:
        //FK: history_citations.history_id -> history.history_id
        e.HasOne(x => x.History)
            .WithMany(x => x.HistoryCitations)
            .HasForeignKey(x => x.HistoryId)
            .OnDelete(DeleteBehavior.Cascade);

        //FK: history_citations.cite_id -> citations.cite_id
        e.HasOne(x => x.Citation)
            .WithMany(x => x.HistoryCitations)
            .HasForeignKey(x => x.CiteId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}