using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace Wars.Resources.Infrastructure.Data;

internal class ResourcesDbContext(DbContextOptions<ResourcesDbContext> options) : DbContext(options)
{
    public required DbSet<Domain.Resources> Resources { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("Resources");
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<decimal>().HavePrecision(18, 6);
    }
}
