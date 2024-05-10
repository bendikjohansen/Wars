namespace Wars.Buildings.Domain;

internal record BuildingUpgrade
{
    public Guid Id { get; private init; }
    public BuildingType Building { get; private init; }
    public ResourceCollection Cost { get; private init; } = ResourceCollection.Empty;
    public TimeSpan Duration { get; private init; }
    public DateTimeOffset StartedAt { get; private init; }

    private BuildingUpgrade() {}

    public static BuildingUpgrade CreateFrom(BuildingType building, ResourceCollection cost, TimeSpan duration,
        DateTimeOffset startedAt)
    {
        return new()
        {
            Id = Guid.NewGuid(),
            Building = building,
            Cost = cost,
            Duration = duration,
            StartedAt = startedAt,
        };
    }

    public bool Started(DateTimeOffset now) => StartedAt <= now;
    public DateTimeOffset FinishedAt => StartedAt + Duration;
    public TimeSpan TimeLeft(DateTimeOffset now) => now - FinishedAt;
    public bool IsFinished(DateTimeOffset now) => TimeLeft(now) > TimeSpan.Zero;
}

internal enum BuildingType
{
    ClayPit,
    IronMine,
    LumberCamp,
    Warehouse,
    Headquarter
}
