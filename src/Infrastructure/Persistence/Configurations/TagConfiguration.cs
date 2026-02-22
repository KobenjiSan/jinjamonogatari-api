using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class TagConfiguration : IEntityTypeConfiguration<Tag>
{
    public void Configure(EntityTypeBuilder<Tag> e)
    {
        // Map entity to table
        e.ToTable("tags");

        // Primary key
        e.HasKey(x => x.TagId);
        e.Property(x => x.TagId).HasColumnName("tag_id").ValueGeneratedOnAdd();

        // Titles
        e.Property(x => x.TitleEn).HasColumnName("title_en").IsRequired();
        e.HasIndex(x => x.TitleEn).IsUnique();

        e.Property(x => x.TitleJp).HasColumnName("title_jp");

        // Timestamps
        e.Property(x => x.CreatedAt).HasColumnName("created_at").HasColumnType("timestamp with time zone").IsRequired();
        e.Property(x => x.UpdatedAt).HasColumnName("updated_at").HasColumnType("timestamp with time zone").IsRequired();
    }
}