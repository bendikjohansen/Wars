using Microsoft.EntityFrameworkCore;
using Wars.Buildings.Domain;

namespace Wars.Buildings.Infrastructure.Data;

internal class EfCoreBuildingsRepository(BuildingsContext context) : IBuildingsRepository
{
    private readonly BuildingsContext _context = context;

    public async Task<Village?> GetAsync(string villageId, CancellationToken ct = default)
        => await _context.Villages
            .Include(v => v.UpgradeQueue)
            .SingleOrDefaultAsync(v => v.Id == villageId, cancellationToken: ct);

    public void Add(Village village)
    {
        _context.Villages.Add(village);
    }

    public Task SaveChangesAsync(CancellationToken ct = default)
    {
        return _context.SaveChangesAsync(ct);
    }
}
