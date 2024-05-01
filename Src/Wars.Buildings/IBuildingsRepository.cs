using Wars.Buildings.Domain;

namespace Wars.Buildings;

internal interface IBuildingsRepository
{
    Task<Village?> GetAsync(string villageId);
    Task SaveChangesAsync(CancellationToken ct);
}
