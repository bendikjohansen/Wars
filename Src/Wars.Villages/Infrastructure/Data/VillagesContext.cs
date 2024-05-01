using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Wars.Villages.Models;

namespace Wars.Villages.Infrastructure.Data;

internal class VillagesContext(DbContextOptions<VillagesContext> options): DbContext(options)
{
    public DbSet<Village> Villages => Set<Village>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.HasDefaultSchema("Villages");
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
