using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

namespace Infrastructure.Persistence.Configurations;

public class EtiquetteTopicConfiguration : IEntityTypeConfiguration<EtiquetteTopic>
{
    public void Configure(EntityTypeBuilder<EtiquetteTopic> e)
    {
        e.ToTable("etiquette_topics");

        e.HasKey(x => x.TopicId);
        e.Property(x => x.TopicId).HasColumnName("topic_id");

        e.Property(x => x.Slug).HasColumnName("slug").IsRequired();
        e.HasIndex(x => x.Slug).IsUnique();

        e.Property(x => x.TitleLong).HasColumnName("title_long");
        e.Property(x => x.TitleShort).HasColumnName("title_short");
        e.Property(x => x.Summary).HasColumnName("summary");

        e.Property(x => x.IconKey).HasColumnName("icon_key");
        e.Property(x => x.IconSet).HasColumnName("icon_set");

        e.Property(x => x.ImageId).HasColumnName("img_id");

        e.Property(x => x.ShowInGlance).HasColumnName("show_in_glance");
        e.Property(x => x.ShowAsHighlight).HasColumnName("show_as_highlight");
        e.Property(x => x.GlanceOrder).HasColumnName("glance_order");
        e.Property(x => x.GuideOrder).HasColumnName("guide_order");

        e.Property(x => x.Status).HasColumnName("status");
        e.Property(x => x.PublishedAt).HasColumnName("published_at").HasColumnType("timestamp with time zone");
        e.Property(x => x.CreatedAt).HasColumnName("created_at").HasColumnType("timestamp with time zone");
        e.Property(x => x.UpdatedAt).HasColumnName("updated_at").HasColumnType("timestamp with time zone");

        e.HasOne(x => x.Image)
         .WithMany(i => i.EtiquetteTopicsAsHero)
         .HasForeignKey(x => x.ImageId)
         .OnDelete(DeleteBehavior.SetNull);

        e.HasMany(x => x.Steps)
         .WithOne(s => s.Topic)
         .HasForeignKey(s => s.TopicId)
         .OnDelete(DeleteBehavior.Cascade);
    }
}