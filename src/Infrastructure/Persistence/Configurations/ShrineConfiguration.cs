using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class ShrineConfiguration : IEntityTypeConfiguration<Shrine>
{
    public void Configure(EntityTypeBuilder<Shrine> builder)
    {
        // This entity maps to "shrines" table
        builder.ToTable("shrines");

        // Define primary key
        builder.HasKey(x => x.ShrineId);
        builder.Property(x => x.ShrineId).HasColumnName("shrine_id");

        builder.Property(x => x.InputtedId).HasColumnName("inputted_id");

        // Explicitly define decimal precision for lat/lon
        // This insures PostgreSQL creates decimal(9,6) instead of default
        builder.Property(x => x.Lat).HasColumnName("lat").HasColumnType("decimal(9,6)");
        builder.Property(x => x.Lon).HasColumnName("lon").HasColumnType("decimal(9,6)");

        // PostGIS
        builder.Property(x => x.Location).HasColumnName("location").HasColumnType("geography(point,4326)");

        // Slug
        builder.Property(x => x.Slug).HasColumnName("slug");

        // create a unique index on slug but onlly when slug is not null
        // this allows drafts to have null slug, but prevents duplicates once set
        builder.HasIndex(x => x.Slug).IsUnique().HasFilter("slug IS NOT NULL");

        // Name + desc
        builder.Property(x => x.NameEn).HasColumnName("name_en");
        builder.Property(x => x.NameJp).HasColumnName("name_jp");
        builder.Property(x => x.ShrineDesc).HasColumnName("shrine_desc");

        // Address
        builder.Property(x => x.AddressRaw).HasColumnName("address_raw");
        builder.Property(x => x.Prefecture).HasColumnName("prefecture");
        builder.Property(x => x.City).HasColumnName("city");
        builder.Property(x => x.Ward).HasColumnName("ward");
        builder.Property(x => x.Locality).HasColumnName("locality");
        builder.Property(x => x.PostalCode).HasColumnName("postal_code");
        builder.Property(x => x.Country).HasColumnName("country");

        // Contact
        builder.Property(x => x.PhoneNumber).HasColumnName("phone_number");
        builder.Property(x => x.Email).HasColumnName("email");
        builder.Property(x => x.Website).HasColumnName("website");

        // Hero image
        builder.Property(x => x.ImgId).HasColumnName("img_id");

        // Publishing state
        builder.Property(x => x.Status).HasColumnName("status");

        // Timestamps 
        builder.Property(x => x.CreatedAt).HasColumnName("created_at").HasColumnType("timestamp with time zone");
        builder.Property(x => x.UpdatedAt).HasColumnName("updated_at").HasColumnType("timestamp with time zone");
        builder.Property(x => x.PublishedAt).HasColumnName("published_at").HasColumnType("timestamp with time zone");

        // Relationship config:
        
        builder.HasOne(x => x.Image)                // shrine has one image
            .WithMany()                             // image can link to many shrines
            .HasForeignKey(x => x.ImgId)            // link to our FK
            .OnDelete(DeleteBehavior.SetNull);      // if images is deleted, set ImgId / Image to null
    }
}