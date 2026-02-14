using API.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options){}

    // Tables (DbSets)
    public DbSet<EtiquetteTopic> EtiquetteTopics => Set<EtiquetteTopic>();
    public DbSet<EtiquetteStep> EtiquetteSteps => Set<EtiquetteStep>();
    public DbSet<EtiquetteTopicCitation> EtiquetteCitations => Set<EtiquetteTopicCitation>();
    public DbSet<Image> Images => Set<Image>();
    public DbSet<Citation> Citations => Set<Citation>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}