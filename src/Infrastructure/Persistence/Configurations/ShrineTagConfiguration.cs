using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class ShrineTagConfiguration : IEntityTypeConfiguration<ShrineTag>
{
    public void Configure(EntityTypeBuilder<ShrineTag> e)
    {
        // Map entity to table
        e.ToTable("shrine_tags");

        // Composite primary key
        e.HasKey(x => new { x.ShrineId, x.TagId});

        // Joined Tables
        e.Property(x => x.ShrineId).HasColumnName("shrine_id").IsRequired();
        e.Property(x => x.TagId).HasColumnName("tag_id").IsRequired();

        // Timestamp
        e.Property(x => x.CreatedAt).HasColumnName("created_at").HasColumnType("timestamp with time zone").IsRequired();

        // Relationships config:
        //FK: shrine_tags.shrine_id -> shrines.shrine_id
        e.HasOne(x => x.Shrine)                     // Each ShrineTag references ONE Shrine
            .WithMany(x => x.ShrineTags)            // A Shrine can have MANY ShrineTag links
            .HasForeignKey(x => x.ShrineId)         // FK column stored on shrine_tags
            .OnDelete(DeleteBehavior.Cascade);      // Deleting a Shrine removes its tag links

        //FK: shrine_tags.tag_id -> Tags.tag_id
        e.HasOne(x => x.Tag)                        // Each ShrineTag references ONE Tag
            .WithMany(x => x.ShrineTags)            // A Tag can have MANY ShrineTag links
            .HasForeignKey(x => x.TagId)            // FK column stored on shrine_tags
            .OnDelete(DeleteBehavior.Cascade);      // Deleting a Tag removes its shrine links
    }
}