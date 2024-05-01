using Microsoft.EntityFrameworkCore;
using Wars.Villages.Domain;

namespace Wars.Villages.Infrastructure.Data;

internal class EfCoreVillagesRepository(VillagesContext context) : IVillagesRepository
{
    private readonly VillagesContext _context = context;

    public async Task<ICollection<Village>> ListByUserAsync(string userId, CancellationToken ct = default)
    {
        return await _context.Villages.Where(village => village.OwnerId == userId).ToListAsync(ct);
    }

    public void Add(Village village)
    {
        _context.Villages.Add(village);
    }

    public Task SaveChangesAsync(CancellationToken ct = default) => _context.SaveChangesAsync(ct);
}
