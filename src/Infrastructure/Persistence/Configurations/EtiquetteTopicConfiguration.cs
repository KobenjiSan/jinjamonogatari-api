using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

namespace Infrastructure.Persistence.Configurations;

public class EtiquetteTopicConfiguration : IEntityTypeConfiguration<EtiquetteTopic>
{
    public void Configure(EntityTypeBuilder<EtiquetteTopic> e)
    {
        // Map entity to table
        e.ToTable("etiquette_topics");

        // Primary key
        e.HasKey(x => x.TopicId);
        e.Property(x => x.TopicId).HasColumnName("topic_id").ValueGeneratedOnAdd();

        // Identifier
        e.Property(x => x.Slug).HasColumnName("slug").IsRequired();
        e.HasIndex(x => x.Slug).IsUnique();

        // Content
        e.Property(x => x.TitleLong).HasColumnName("title_long").HasMaxLength(30);
        e.Property(x => x.Summary).HasColumnName("summary");

        // At a Glance
        e.Property(x => x.ShowInGlance).HasColumnName("show_in_glance");
        e.Property(x => x.TitleShort).HasColumnName("title_short").HasMaxLength(12);
        e.Property(x => x.IconKey).HasColumnName("icon_key");
        e.Property(x => x.IconSet).HasColumnName("icon_set");
        e.Property(x => x.GlanceOrder).HasColumnName("glance_order");

        // UI positioning
        e.Property(x => x.ShowAsHighlight).HasColumnName("show_as_highlight");
        e.Property(x => x.GuideOrder).HasColumnName("guide_order");

        // Publishing
        e.Property(x => x.Status).HasColumnName("status");

        // Timestamps
        e.Property(x => x.PublishedAt).HasColumnName("published_at").HasColumnType("timestamp with time zone");
        e.Property(x => x.CreatedAt).HasColumnName("created_at").HasColumnType("timestamp with time zone").IsRequired();
        e.Property(x => x.UpdatedAt).HasColumnName("updated_at").HasColumnType("timestamp with time zone").IsRequired();

        // Relationship config:
        e.HasMany(x => x.Steps)
            .WithOne(s => s.Topic)
            .HasForeignKey(s => s.TopicId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}