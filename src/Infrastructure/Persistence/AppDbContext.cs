using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Persistence.Entities;
using Application.Common.Interfaces;

namespace Infrastructure;

public class AppDbContext : DbContext, IAppDbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options){}

    // Tables (DbSets)
    public DbSet<User> Users => Set<User>();
    public DbSet<EtiquetteTopic> EtiquetteTopics => Set<EtiquetteTopic>();
    public DbSet<EtiquetteStep> EtiquetteSteps => Set<EtiquetteStep>();
    public DbSet<EtiquetteTopicCitation> EtiquetteCitations => Set<EtiquetteTopicCitation>();
    public DbSet<Image> Images => Set<Image>();
    public DbSet<Citation> Citations => Set<Citation>();

    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}