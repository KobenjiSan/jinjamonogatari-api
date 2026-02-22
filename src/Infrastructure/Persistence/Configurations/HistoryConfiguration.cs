using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class HistoryConfiguration : IEntityTypeConfiguration<History>
{
    public void Configure(EntityTypeBuilder<History> e)
    {
        // Map entity to table
        e.ToTable("history");

        // Primary key
        e.HasKey(x => x.HistoryId);
        e.Property(x => x.HistoryId).HasColumnName("history_id").ValueGeneratedOnAdd();

        // Shrine FK
        e.Property(x => x.ShrineId).HasColumnName("shrine_id").IsRequired();

        // Content
        e.Property(x => x.EventDate).HasColumnName("event_date");
        e.Property(x => x.SortOrder).HasColumnName("sort_order");

        e.Property(x => x.Title).HasColumnName("title");
        e.Property(x => x.Information).HasColumnName("information");

        // Main image
        e.Property(x => x.ImgId).HasColumnName("img_id");

        // Publishing state
        e.Property(x => x.Status).HasColumnName("status");

        // Timestamps
        e.Property(x => x.PublishedAt).HasColumnName("published_at").HasColumnType("timestamp with time zone");
        e.Property(x => x.CreatedAt).HasColumnName("created_at").HasColumnType("timestamp with time zone").IsRequired();
        e.Property(x => x.UpdatedAt).HasColumnName("updated_at").HasColumnType("timestamp with time zone").IsRequired();

        // Relationship config:
        //FK: history.shrine_id -> shrines.shrine_id
        e.HasOne(x => x.Shrine)
            .WithMany(x => x.ShrineHistories)
            .HasForeignKey(x => x.ShrineId)
            .OnDelete(DeleteBehavior.Cascade);      // When shrine is deleted history will also be deleted

        //FK: history.img_id -> images.img_id
        e.HasOne(x => x.Image)
            .WithMany()
            .HasForeignKey(x => x.ImgId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}