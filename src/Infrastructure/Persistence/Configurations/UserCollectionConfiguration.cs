using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class UserCollectionConfiguration : IEntityTypeConfiguration<UserCollection>
{
    public void Configure(EntityTypeBuilder<UserCollection> e)
    {
        e.ToTable("user_collection");

        e.HasKey(x => new { x.UserId, x.ShrineId });

        e.Property(x => x.UserId).HasColumnName("user_id").IsRequired();
        e.Property(x => x.ShrineId).HasColumnName("shrine_id").IsRequired();

        // Timestamp
        e.Property(x => x.CreatedAt).HasColumnName("created_at").HasColumnType("timestamp with time zone").IsRequired();

        // Relationship config:
        e.HasOne(x => x.Shrine)
            .WithMany(x => x.UserCollections)
            .HasForeignKey(x => x.ShrineId)
            .OnDelete(DeleteBehavior.Cascade);

        e.HasOne(x => x.User)
            .WithMany(x => x.UserCollections)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}