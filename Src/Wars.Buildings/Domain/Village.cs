namespace Wars.Buildings.Domain;

internal record Village
{
    public string Id { get; private init; } = string.Empty;
    public BuildingLevelRegistry BuildingLevels { get; } = new();

    private readonly List<BuildingUpgrade> _jobs = [];
    public List<BuildingUpgrade> UpgradeQueue => _jobs.ToList();

    private Village() {}

    public static Village CreateFrom(string id)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(id);

        return new Village
        {
            Id = id,
        };
    }

    public void QueueUpgrade(BuildingType building, DateTimeOffset now, BuildingDurationLookup durationLookup,
        BuildingCostLookup costLookup)
    {
        var buildingLevel = GetBuildingLevelAfterQueue(building) + 1;
        var upgradeCost = costLookup(building, buildingLevel);
        var duration = durationLookup(building, buildingLevel);
        var newJob = BuildingUpgrade.CreateFrom(building, upgradeCost, duration, now);
        _jobs.Add(newJob);
    }

    public void ProcessFinishedUpgrades(DateTimeOffset now)
    {
        var finishedJobs = _jobs.TakeWhile(job => job.IsFinished(now)).ToArray();
        _jobs.RemoveAll(job => finishedJobs.Contains(job));

        foreach (var job in finishedJobs)
        {
            BuildingLevels.IncreaseLevel(job.Building);
        }
    }

    public int GetBuildingLevelAfterQueue(BuildingType building)
    {
        var levelDelta = UpgradeQueue.Count(job => job.Building == building);
        var buildingLevel = levelDelta + building switch
        {
            BuildingType.Headquarter => BuildingLevels.Headquarter,
            BuildingType.Warehouse => BuildingLevels.Warehouse,
            BuildingType.IronMine => BuildingLevels.IronMine,
            BuildingType.ClayPit => BuildingLevels.ClayPit,
            BuildingType.LumberCamp => BuildingLevels.LumberCamp,
            _ => throw new NotSupportedException($"No such building: {building}")
        };
        return buildingLevel;
    }
}

internal record BuildingLevelRegistry
{
    public int ClayPit { get; private set; } = 3;
    public int LumberCamp { get; private set; } = 3;
    public int IronMine { get; private set; } = 3;
    public int Warehouse { get; private set; } = 1;
    public int Headquarter { get; private set; } = 1;


    public void IncreaseLevel(BuildingType building)
    {
        if (building == BuildingType.Headquarter)
        {
            Headquarter += 1;
        }
        else if (building == BuildingType.Warehouse)
        {
            Warehouse += 1;
        }
        else if (building == BuildingType.IronMine)
        {
            IronMine += 1;
        }
        else if (building == BuildingType.ClayPit)
        {
            ClayPit += 1;
        }
        else if (building == BuildingType.LumberCamp)
        {
            LumberCamp += 1;
        }
    }
}
