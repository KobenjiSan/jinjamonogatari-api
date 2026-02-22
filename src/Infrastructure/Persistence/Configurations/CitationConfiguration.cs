using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

namespace Infrastructure.Persistence.Configurations;

public class CitationConfiguration : IEntityTypeConfiguration<Citation>
{
    public void Configure(EntityTypeBuilder<Citation> e)
    {
        // Map entity to table
        e.ToTable("citations");

        // Primary key
        e.HasKey(x => x.CiteId);
        e.Property(x => x.CiteId).HasColumnName("cite_id").ValueGeneratedOnAdd();

        // Content
        e.Property(x => x.Title).HasColumnName("title");
        e.Property(x => x.Author).HasColumnName("author");
        e.Property(x => x.Url).HasColumnName("url");
        e.Property(x => x.Year).HasColumnName("year");

        // Editor Notes
        e.Property(x => x.Notes).HasColumnName("notes");

        // Timestamps
        e.Property(x => x.CreatedAt).HasColumnName("created_at").HasColumnType("timestamp with time zone").IsRequired();
        e.Property(x => x.UpdatedAt).HasColumnName("updated_at").HasColumnType("timestamp with time zone").IsRequired();
    }
}