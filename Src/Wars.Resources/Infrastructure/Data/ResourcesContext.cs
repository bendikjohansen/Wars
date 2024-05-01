using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Wars.Resources.Domain;

namespace Wars.Resources.Infrastructure.Data;

internal class ResourcesContext(DbContextOptions<ResourcesContext> options) : DbContext(options)
{
    public DbSet<Village> Villages => Set<Village>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("Resources");
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<float>().HavePrecision(18, 6);
    }
}
