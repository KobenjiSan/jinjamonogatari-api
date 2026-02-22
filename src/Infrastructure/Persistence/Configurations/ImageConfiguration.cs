using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

namespace Infrastructure.Persistence.Configurations;

public class ImageConfiguration : IEntityTypeConfiguration<Image>
{
    public void Configure(EntityTypeBuilder<Image> e)
    {
        // Map entity to table
        e.ToTable("images");

        // Primary key
        e.HasKey(x => x.ImgId);
        e.Property(x => x.ImgId).HasColumnName("img_id").ValueGeneratedOnAdd();

        // Content
        e.Property(x => x.ImgSource).HasColumnName("img_source");
        e.Property(x => x.Title).HasColumnName("title");
        e.Property(x => x.Desc).HasColumnName("desc");

        // Citation
        e.Property(x => x.CiteId).HasColumnName("cite_id");

        // Timestamps
        e.Property(x => x.CreatedAt).HasColumnName("created_at").HasColumnType("timestamp with time zone").IsRequired();
        e.Property(x => x.UpdatedAt).HasColumnName("updated_at").HasColumnType("timestamp with time zone").IsRequired();

        // Relationship config:
        e.HasOne(x => x.Citation)
            .WithMany(c => c.ImagesAttributed)
            .HasForeignKey(x => x.CiteId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}