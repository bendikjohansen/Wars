namespace Wars.Buildings.Domain;

internal record BuildingJob
{
    public Guid Id { get; private init; } = Guid.NewGuid();
    public required BuildingType Building { get; init; }
    public required ResourceCollection Cost { get; init; }
    public required TimeSpan Duration { get; init; }
    public required DateTimeOffset StartedAt { get; init; }


    public DateTimeOffset FinishedAt => StartedAt + Duration;
    public bool IsFinished(DateTimeOffset now) => FinishedAt <= now;
}

internal enum BuildingType
{
    ClayPit,
    IronMine,
    LumberCamp,
    Warehouse,
    Headquarter
}
