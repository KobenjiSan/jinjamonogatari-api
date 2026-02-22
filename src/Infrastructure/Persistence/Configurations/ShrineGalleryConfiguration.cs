using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class ShrineGalleryConfiguration : IEntityTypeConfiguration<ShrineGallery>
{
    public void Configure(EntityTypeBuilder<ShrineGallery> e)
    {
        // Map entity to table
        e.ToTable("shrine_gallery");

        // Composite primary key
        e.HasKey(x => new { x.ShrineId , x.ImgId });

        // Joined Tables
        e.Property(x => x.ShrineId).HasColumnName("shrine_id").IsRequired();
        e.Property(x => x.ImgId).HasColumnName("img_id").IsRequired();

        // Timestamp
        e.Property(x => x.CreatedAt).HasColumnName("created_at").HasColumnType("timestamp with time zone").IsRequired();

        // Relationships
        //FK: shrine_gallery.shrine_id -> shrines.shrine_id
        e.HasOne(x => x.Shrine)
            .WithMany(x => x.ShrineGalleries)
            .HasForeignKey(x => x.ShrineId)
            .OnDelete(DeleteBehavior.Cascade);

        e.HasOne(x => x.Image)
            .WithMany()
            .HasForeignKey(x => x.ImgId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}