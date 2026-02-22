using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class ShrineConfiguration : IEntityTypeConfiguration<Shrine>
{
    public void Configure(EntityTypeBuilder<Shrine> e)
    {
        // This entity maps to "shrines" table
        e.ToTable("shrines");

        // Define primary key
        e.HasKey(x => x.ShrineId);
        e.Property(x => x.ShrineId).HasColumnName("shrine_id").ValueGeneratedOnAdd();

        e.Property(x => x.InputtedId).HasColumnName("inputted_id");

        // Explicitly define decimal precision for lat/lon
        // This insures PostgreSQL creates decimal(9,6) instead of default
        e.Property(x => x.Lat).HasColumnName("lat").HasColumnType("decimal(9,6)");
        e.Property(x => x.Lon).HasColumnName("lon").HasColumnType("decimal(9,6)");

        // PostGIS
        e.Property(x => x.Location).HasColumnName("location").HasColumnType("geography(point,4326)");

        // Slug
        e.Property(x => x.Slug).HasColumnName("slug");

        // create a unique index on slug but onlly when slug is not null
        // this allows drafts to have null slug, but prevents duplicates once set
        e.HasIndex(x => x.Slug).IsUnique().HasFilter("slug IS NOT NULL");

        // Name + desc
        e.Property(x => x.NameEn).HasColumnName("name_en");
        e.Property(x => x.NameJp).HasColumnName("name_jp");
        e.Property(x => x.ShrineDesc).HasColumnName("shrine_desc");

        // Address
        e.Property(x => x.AddressRaw).HasColumnName("address_raw");
        e.Property(x => x.Prefecture).HasColumnName("prefecture");
        e.Property(x => x.City).HasColumnName("city");
        e.Property(x => x.Ward).HasColumnName("ward");
        e.Property(x => x.Locality).HasColumnName("locality");
        e.Property(x => x.PostalCode).HasColumnName("postal_code");
        e.Property(x => x.Country).HasColumnName("country");

        // Contact
        e.Property(x => x.PhoneNumber).HasColumnName("phone_number");
        e.Property(x => x.Email).HasColumnName("email");
        e.Property(x => x.Website).HasColumnName("website");

        // Hero image
        e.Property(x => x.ImgId).HasColumnName("img_id");

        // Publishing state
        e.Property(x => x.Status).HasColumnName("status");

        // Timestamps 
        e.Property(x => x.PublishedAt).HasColumnName("published_at").HasColumnType("timestamp with time zone");
        e.Property(x => x.CreatedAt).HasColumnName("created_at").HasColumnType("timestamp with time zone").IsRequired();
        e.Property(x => x.UpdatedAt).HasColumnName("updated_at").HasColumnType("timestamp with time zone").IsRequired();

        // Relationship config:
        
        e.HasOne(x => x.Image)                      // shrine has one image
            .WithMany()                             // image can link to many shrines
            .HasForeignKey(x => x.ImgId)            // link to our FK
            .OnDelete(DeleteBehavior.SetNull);      // if images is deleted, set ImgId / Image to null
    }
}