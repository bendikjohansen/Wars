namespace Wars.Resources.Domain;

internal class Resources
{
    public Guid Id { get; } = Guid.NewGuid();
    public required string VillageId { get; init; }

    public decimal Clay { get; private set; }
    public decimal Iron { get; private set; }
    public decimal Wood { get; private set; }

    public int WarehouseCapacity { get; private set; } = 100;

    public DateTimeOffset LastUpdated { get; private init; } = DateTimeOffset.UtcNow;
}
