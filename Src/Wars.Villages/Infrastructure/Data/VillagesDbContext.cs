using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Wars.Common;
using Wars.Villages.Domain;

namespace Wars.Villages.Infrastructure.Data;

internal class VillagesDbContext(DbContextOptions<VillagesDbContext> options, IDomainEventDispatcher? dispatcher): DbContext(options)
{
    public required DbSet<Village> Villages { get; init; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.HasDefaultSchema("Villages");
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    public override async Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        var result = await base.SaveChangesAsync(ct);

        if (dispatcher == null)
        {
            return result;
        }

        var entitiesWithEvents = ChangeTracker.Entries<IHaveDomainEvents>()
            .Select(e => e.Entity)
            .Where(e => e.DomainEvents.Any())
            .ToArray();
        await dispatcher.DispatchAndClearEventsAsync(entitiesWithEvents, ct);

        return result;
    }
}