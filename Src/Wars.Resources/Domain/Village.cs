
namespace Wars.Resources.Domain;

internal record Village
{
    public required string Id { get; init; }
    public ResourceInventory ResourceInventory { get; } = new(DateTimeOffset.Now);
    public ResourceBuilding ResourceBuilding { get; } = new();

    private Village() {}
    public Village(DateTimeOffset now)
    {
        ResourceInventory = new ResourceInventory(now);
    }

    public void CollectResources(DateTimeOffset now)
    {
        var secondsElapsed = ResourceInventory.UpdatedAt.Subtract(now).Duration();
        var resourcesCollected = ResourceBuilding.Collect(secondsElapsed);
        ResourceInventory.Add(resourcesCollected, now);
    }

    public bool HasFunds(int clay, int iron, int wood) => ResourceInventory.Contains(clay, iron, wood);

    public void Pay(int clay, int iron, int wood, DateTimeOffset now)
    {
        ResourceInventory.Pay(clay, iron, wood, now);
    }
}

internal record ResourceInventory
{
    public float Clay { get; private set; }
    public float Iron { get; private set; }
    public float Wood { get; private set; }

    public int WarehouseCapacity { get; private set; } = 100;
    public DateTimeOffset UpdatedAt { get; private set; }

    private ResourceInventory() { }

    public ResourceInventory(DateTimeOffset now)
    {
        UpdatedAt = now;
    }

    public void Add(ResourceCollection collection, DateTimeOffset now)
    {
        Clay = Math.Min(Clay + collection.Clay, WarehouseCapacity);
        Iron = Math.Min(Iron + collection.Iron, WarehouseCapacity);
        Wood = Math.Min(Wood + collection.Wood, WarehouseCapacity);
        UpdatedAt = now;
    }

    public bool Contains(int clay, int iron, int wood) =>
        Clay >= clay && Iron >= iron && Wood >= wood;

    public void Pay(int clay, int iron, int wood, DateTimeOffset now)
    {
        Clay -= clay;
        Iron -= iron;
        Wood -= wood;
        UpdatedAt = now;
    }
}

internal record ResourceBuilding
{
    public int ClayPit { get; private set; } = 3;
    public int IronMine { get; private set; } = 3;
    public int LumberCamp { get; private set; } = 3;

    public ResourceCollection Collect(TimeSpan timeSpan)
    {
        const int gatheringRatePerMinute = 6;
        var resourcesGathered = (float)timeSpan.TotalMinutes * gatheringRatePerMinute;

        return new ResourceCollection
        {
            Wood = resourcesGathered * LumberCamp,
            Clay = resourcesGathered * ClayPit,
            Iron = resourcesGathered * IronMine
        };
    }
}

internal record ResourceCollection
{
    public required float Wood { get; init; }
    public required float Iron { get; init; }
    public required float Clay { get; init; }
}
