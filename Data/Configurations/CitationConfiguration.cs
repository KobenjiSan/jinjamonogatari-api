using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using API.Domain.Entities;

namespace API.Data.Configurations;

public class CitationConfiguration : IEntityTypeConfiguration<Citation>
{
    public void Configure(EntityTypeBuilder<Citation> e)
    {
        e.ToTable("citations");

        e.HasKey(x => x.CiteId);
        e.Property(x => x.CiteId).HasColumnName("cite_id");

        e.Property(x => x.Title).HasColumnName("title");
        e.Property(x => x.Author).HasColumnName("author");
        e.Property(x => x.Url).HasColumnName("url");
        e.Property(x => x.Year).HasColumnName("year");
        e.Property(x => x.Notes).HasColumnName("notes");
    }
}