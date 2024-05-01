using Microsoft.EntityFrameworkCore;
using Wars.Resources.Domain;

namespace Wars.Resources.Infrastructure.Data;

internal class EfCoreResourceRepository(ResourcesContext context) : IResourcesRepository
{
    private readonly ResourcesContext _context = context;

    public Task<Village?> GetAsync(string id, CancellationToken ct = default)
        => _context.Villages.SingleOrDefaultAsync(r => r.Id == id, ct);

    public void Add(Village village)
    {
        _context.Villages.Add(village);
    }

    public Task SaveChangesAsync(CancellationToken ct = default) => _context.SaveChangesAsync(ct);
}
