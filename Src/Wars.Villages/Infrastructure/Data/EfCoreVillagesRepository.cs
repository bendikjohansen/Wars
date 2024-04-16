using Microsoft.EntityFrameworkCore;
using Wars.Villages.Domain;

namespace Wars.Villages.Infrastructure.Data;

internal class EfCoreVillagesRepository(VillagesDbContext dbContext) : IVillagesRepository
{
    private readonly VillagesDbContext _dbContext = dbContext;

    public async Task<ICollection<Village>> ListByUserAsync(string userId, CancellationToken ct = default)
    {
        return await _dbContext.Villages.Where(village => village.OwnerId == userId).ToListAsync(ct);
    }

    public void Add(Village village)
    {
        _dbContext.Villages.Add(village);
    }

    public Task SaveChangesAsync(CancellationToken ct = default) => _dbContext.SaveChangesAsync(ct);
}
