using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> b)
    {
        b.ToTable("RefreshTokens");

        b.HasKey(x => x.Id);

        b.Property(x => x.TokenHash)
            .IsRequired();

        b.Property(x => x.ExpiresAtUtc)
            .IsRequired();

        b.Property(x => x.CreatedAtUtc)
            .IsRequired();

        b.HasIndex(x => x.TokenHash)
            .IsUnique();

        b.HasIndex(x => x.UserId);

        b.HasOne(x => x.User)
            .WithMany(u => u.RefreshTokens)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}