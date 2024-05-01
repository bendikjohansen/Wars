using Wars.Buildings.Domain;

namespace Wars.Buildings.Infrastructure.Data;

internal class BuildingsRepository(BuildingsContext context) : IBuildingsRepository
{
    private readonly BuildingsContext _context = context;

    public async Task<Village?> GetAsync(string villageId) => await _context.Villages.FindAsync(villageId);

    public Task SaveChangesAsync(CancellationToken ct) => _context.SaveChangesAsync(ct);
}
