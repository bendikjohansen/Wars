namespace Wars.Buildings.Domain;

internal record Village
{
    public required string Id { get; init; }
    public BuildingLevelRegistry BuildingLevels { get; private init; } = new();

    private readonly List<BuildingJob> _jobs = [];
    public IEnumerable<BuildingJob> Jobs => _jobs.ToArray();

    public void QueueUpgrade(BuildingType building, BuildingCost buildingCost)
    {
        var buildingLevel = GetBuildingLevelAfterQueue(building) + 1;
        if (buildingLevel >= 30)
        {
            throw new Exception("Building cannot be upgraded beyond level 30");
        }

        var upgradeCost = buildingCost.ForUpgrading(building, buildingLevel);
        var newJob = new BuildingJob
        {
            Building = building,
            Cost = upgradeCost,
            JobType = JobType.Upgrade,
            Duration = TimeSpan.FromMinutes(Math.Pow(buildingLevel, 2))
        };
        _jobs.Add(newJob);
    }

    public void QueueDemolition(BuildingType building, BuildingCost buildingCost)
    {
        var buildingLevel = GetBuildingLevelAfterQueue(building) - 1;
        if (buildingLevel <= 1)
        {
            throw new Exception("Building cannot be demolished to less than level 1");
        }

        var recycleValue = buildingCost.ForDemolishing(building, buildingLevel);
        var newJob = new BuildingJob
        {
            Building = building,
            Cost = recycleValue,
            JobType = JobType.Demolish,
            Duration = TimeSpan.FromMinutes(Math.Pow(buildingLevel, 1.5))
        };
        _jobs.Add(newJob);
    }

    public int GetBuildingLevelAfterQueue(BuildingType building)
    {
        var levelDelta = Jobs
            .Where(job => job.Building == building)
            .Sum(job => job.JobType == JobType.Demolish ? -1 : 1);
        var buildingLevel = levelDelta + building switch
        {
            BuildingType.Headquarter => BuildingLevels.Headquarter,
            BuildingType.Warehouse => BuildingLevels.Warehouse,
            BuildingType.IronMine => BuildingLevels.IronMine,
            BuildingType.ClayPit => BuildingLevels.ClayPit,
            BuildingType.LumberCamp => BuildingLevels.LumberCamp,
            _ => throw new Exception($"No such building: {building}")
        };
        return buildingLevel;
    }
}

internal record BuildingLevelRegistry
{
    public int ClayPit { get; set; } = 3;
    public int LumberCamp { get; set; } = 3;
    public int IronMine { get; set; } = 3;
    public int Warehouse { get; set; } = 1;
    public int Headquarter { get; set; } = 1;
}
