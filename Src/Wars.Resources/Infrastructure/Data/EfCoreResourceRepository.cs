using Microsoft.EntityFrameworkCore;

namespace Wars.Resources.Infrastructure.Data;

internal class EfCoreResourceRepository(ResourcesDbContext dbContext) : IResourcesRepository
{
    private readonly ResourcesDbContext _dbContext = dbContext;

    public Task<Domain.Resources?> GetAsync(string villageId, CancellationToken ct = default)
        => _dbContext.Resources.SingleOrDefaultAsync(r => r.VillageId == villageId, ct);

    public void Add(Domain.Resources resources)
    {
        _dbContext.Resources.Add(resources);
    }

    public Task SaveChangesAsync(CancellationToken ct = default) => _dbContext.SaveChangesAsync(ct);
}
