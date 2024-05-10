using Wars.Buildings.Domain;

namespace Wars.Buildings;

internal interface IBuildingsRepository
{
    Task<Village?> GetAsync(string villageId, CancellationToken ct = default);
    void Add(Village village);
    Task SaveChangesAsync(CancellationToken ct = default);
}
