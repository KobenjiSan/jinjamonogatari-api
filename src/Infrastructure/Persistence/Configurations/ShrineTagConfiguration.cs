using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class ShrineTagConfiguration : IEntityTypeConfiguration<ShrineTag>
{
    public void Configure(EntityTypeBuilder<ShrineTag> builder)
    {
        // Map entity to table
        builder.ToTable("shrine_tags");

        // Composite primary key
        builder.HasKey(x => new { x.ShrineId, x.TagId});

        // Column names
        builder.Property(x => x.ShrineId).HasColumnName("shrine_id");
        builder.Property(x => x.TagId).HasColumnName("tag_id");

        // Timestamp
        builder.Property(x => x.CreatedAt).HasColumnName("created_at").HasColumnType("timestamp with time zone");

        // Relationships
        //FK: shrine_tags.shrine_id -> shrines.shrine_id
        builder.HasOne(x => x.Shrine)               // Each ShrineTag references ONE Shrine
            .WithMany(x => x.ShrineTags)            // A Shrine can have MANY ShrineTag links
            .HasForeignKey(x => x.ShrineId)         // FK column stored on shrine_tags
            .OnDelete(DeleteBehavior.Cascade);      // Deleting a Shrine removes its tag links

        //FK: shrine_tags.tag_id -> Tags.tag_id
        builder.HasOne(x => x.Tag)                  // Each ShrineTag references ONE Tag
            .WithMany(x => x.ShrineTags)            // A Tag can have MANY ShrineTag links
            .HasForeignKey(x => x.TagId)            // FK column stored on shrine_tags
            .OnDelete(DeleteBehavior.Cascade);      // Deleting a Tag removes its shrine links
    }
}