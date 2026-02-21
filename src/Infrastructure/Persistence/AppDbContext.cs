using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Persistence.Entities;
using Application.Common.Interfaces;
using Domain.Common;

namespace Infrastructure;

public class AppDbContext : DbContext, IAppDbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options){}

    // DbSets (tables EF Core will track)
    // Shrines
    public DbSet<Shrine> Shrines => Set<Shrine>();

    // Tags
    public DbSet<Tag> Tags => Set<Tag>();
    public DbSet<ShrineTag> ShrineTags => Set<ShrineTag>();

    // Auth
    public DbSet<User> Users => Set<User>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

    // Etiquette
    public DbSet<EtiquetteTopic> EtiquetteTopics => Set<EtiquetteTopic>();
    public DbSet<EtiquetteStep> EtiquetteSteps => Set<EtiquetteStep>();
    public DbSet<EtiquetteTopicCitation> EtiquetteCitations => Set<EtiquetteTopicCitation>();

    // Images
    public DbSet<Image> Images => Set<Image>();

    // Citations
    public DbSet<Citation> Citations => Set<Citation>();

    /*
    Builds EF Core model at startup.
    Applies all IEntityTypeConfiguration mappings (table names,
    relationships, column rules, indexes, etc.) automatically.
    */
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }

    /*
    Intercepts EF Core save operations and automatically applies timestamp values.
    - Sets CreatedAt when entities are first inserted.
    - Updates UpdatedAt whenever tracked entities are modified.
    This keeps timestamp logic centralized in the persistence layer
    instead of duplicating it across services or handlers.
    */
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var now = DateTime.UtcNow;

        var entries = ChangeTracker.Entries()
            .Where(e => e.State is EntityState.Added or EntityState.Modified);

        foreach(var entry in entries)
        {
            // Update Location from Lat / Lon
            if (entry.Entity is Shrine shrine)
            {
                // If coords exist, ensure Location stays in sync.
                // Point uses (x,y) = (Lon,Lat). SRID 4326 = GPS/WGS84
                if(shrine.Lat.HasValue && shrine.Lon.HasValue)
                {
                    shrine.Location = new NetTopologySuite.Geometries.Point((double) shrine.Lon.Value, (double)shrine.Lat.Value)
                    {
                        SRID = 4326
                    };
                }
                else
                {
                    shrine.Location = null;
                }
            }

            // Update CreatedAt / UpdatedAt
            if(entry.Entity is IHasTimestamps ts)
            {
                if(entry.State == EntityState.Added)
                    ts.CreatedAt = now;

                ts.UpdatedAt = now;
            }
            else if(entry.Entity is IHasCreatedAt created && entry.State == EntityState.Added)
            {
                created.CreatedAt = now;
            }
        }
        return await base.SaveChangesAsync(cancellationToken);
    }
}