using Microsoft.EntityFrameworkCore;
using Wars.Buildings.Domain;

namespace Wars.Buildings.Infrastructure.Data;

internal class BuildingsContext(DbContextOptions<BuildingsContext> options) : DbContext(options)
{
    public DbSet<Village> Villages => Set<Village>();
}
