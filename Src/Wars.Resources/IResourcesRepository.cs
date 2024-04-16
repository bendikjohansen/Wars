namespace Wars.Resources;

internal interface IResourcesRepository
{
    Task<Domain.Resources?> GetAsync(string villageId, CancellationToken ct = default);
    void Add(Domain.Resources resources);
    Task SaveChangesAsync(CancellationToken ct = default);
}
