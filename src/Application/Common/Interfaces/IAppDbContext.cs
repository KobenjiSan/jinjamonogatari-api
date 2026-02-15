using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Common.Interfaces;

public interface IAppDbContext
{
    // Exposes only the tables Application logic is allowed to use.
    // This prevents Application from depending on Infrastructure directly.
    DbSet<User> Users { get; }

    // Save changes to the database.
    // Handlers call this after making changes.
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

}