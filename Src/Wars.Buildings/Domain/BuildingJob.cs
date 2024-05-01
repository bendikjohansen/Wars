namespace Wars.Buildings.Domain;

internal record BuildingJob
{
    public Guid Id { get; private init; } = Guid.NewGuid();
    public required JobType JobType { get; init; }
    public required BuildingType Building { get; init; }
    public required ResourceCollection Cost { get; init; }
    public required TimeSpan Duration { get; init; }
    public DateTimeOffset StartedAt { get; init; }
    public JobStatus Status { get; private set; } = JobStatus.Pending;
}

internal enum BuildingType
{
    ClayPit,
    IronMine,
    LumberCamp,
    Warehouse,
    Headquarter
}

internal enum JobType
{
    Upgrade,
    Demolish
}

internal enum JobStatus
{
    Pending,
    Completed
}
