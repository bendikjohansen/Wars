using Microsoft.EntityFrameworkCore;
using Wars.Buildings.Domain;

namespace Wars.Buildings.Infrastructure.Data;

internal class BuildingsContext(DbContextOptions<BuildingsContext> options) : DbContext(options)
{
    public DbSet<Village> Villages => Set<Village>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("Buildings");

        modelBuilder.Entity<Village>(builder =>
        {
            builder.Property(v => v.Id).ValueGeneratedNever();
            builder.OwnsOne(v => v.BuildingLevels);
        });
        modelBuilder.Entity<BuildingUpgrade>(builder =>
        {
            builder.Property(bu => bu.Id).ValueGeneratedNever();
            builder.OwnsOne(bu => bu.Cost);
        });
    }
}
