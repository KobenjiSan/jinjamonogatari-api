using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class ShrineKamiConfiguration : IEntityTypeConfiguration<ShrineKami>
{
    public void Configure(EntityTypeBuilder<ShrineKami> e)
    {
        // Map entity to table
        e.ToTable("shrine_kami");

        // Composite primary key
        e.HasKey(x => new { x.ShrineId, x.KamiId });

        // Joined Tables
        e.Property(x => x.ShrineId).HasColumnName("shrine_id").IsRequired();
        e.Property(x => x.KamiId).HasColumnName("kami_id").IsRequired();

        // Timestamp
        e.Property(x => x.CreatedAt).HasColumnName("created_at").HasColumnType("timestamp with time zone").IsRequired();

        // Relationships
        //FK: shrine_kami.shrine_id -> shrines.shrine_id
        e.HasOne(x => x.Shrine)                     // Each ShrineKami references ONE Shrine
            .WithMany(x => x.ShrineKamis)           // A Shrine can have MANY ShrineKami links
            .HasForeignKey(x => x.ShrineId)         // FK column stored on shrine_kami
            .OnDelete(DeleteBehavior.Cascade);      // Deleting a Shrine removes its kami links

        //FK: shrine_kami.kami_id -> kami.kami_id
        e.HasOne(x => x.Kami)                       // Each ShrineKami references ONE Kami
            .WithMany(x => x.ShrineKamis)           // A Kami can have MANY ShrineKami links
            .HasForeignKey(x => x.KamiId)           // FK column stored on shrine_kami
            .OnDelete(DeleteBehavior.Cascade);      // Deleting a Kami removes its shrine links
    }
}