using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class TagConfiguration : IEntityTypeConfiguration<Tag>
{
    public void Configure(EntityTypeBuilder<Tag> builder)
    {
        // Map entity to table
        builder.ToTable("tags");

        // Primary key
        builder.HasKey(x => x.TagId);
        builder.Property(x => x.TagId).HasColumnName("tag_id");

        // Titles
        builder.Property(x => x.TitleEn).HasColumnName("title_en").IsRequired();
        builder.HasIndex(x => x.TitleEn).IsUnique();

        builder.Property(x => x.TitleJp).HasColumnName("title_jp");

        // Timestamps
        builder.Property(x => x.CreatedAt).HasColumnName("created_at").HasColumnType("timestamp with time zone");
        builder.Property(x => x.UpdatedAt).HasColumnName("updated_at").HasColumnType("timestamp with time zone");
    }
}