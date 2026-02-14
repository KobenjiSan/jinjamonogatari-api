using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using API.Domain.Entities;

namespace API.Data.Configurations;

public class ImageConfiguration : IEntityTypeConfiguration<Image>
{
    public void Configure(EntityTypeBuilder<Image> e)
    {
        e.ToTable("images");

        e.HasKey(x => x.ImgId);
        e.Property(x => x.ImgId).HasColumnName("img_id");

        e.Property(x => x.ImgSource).HasColumnName("img_source");
        e.Property(x => x.Title).HasColumnName("title");
        e.Property(x => x.Desc).HasColumnName("desc");

        e.Property(x => x.CiteId).HasColumnName("cite_id");

        e.Property(x => x.CreatedAt).HasColumnName("created_at").HasColumnType("timestamp with time zone");
        e.Property(x => x.UpdatedAt).HasColumnName("updated_at").HasColumnType("timestamp with time zone");

        e.HasOne(x => x.Citation)
         .WithMany(c => c.ImagesAttributed)
         .HasForeignKey(x => x.CiteId)
         .OnDelete(DeleteBehavior.SetNull);
    }
}