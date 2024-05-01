namespace Wars.Resources;

internal interface IResourcesRepository
{
    Task<Domain.Village?> GetAsync(string id, CancellationToken ct = default);
    void Add(Domain.Village village);
    Task SaveChangesAsync(CancellationToken ct = default);
}
