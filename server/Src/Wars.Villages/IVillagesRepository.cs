using Wars.Villages.Models;

namespace Wars.Villages;

internal interface IVillagesRepository
{
    Task<ICollection<Village>> ListByUserAsync(string userId, CancellationToken ct = default);
    void Add(Village village);
    Task SaveChangesAsync(CancellationToken ct = default);
}
