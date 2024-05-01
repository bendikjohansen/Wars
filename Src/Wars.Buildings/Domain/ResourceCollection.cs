namespace Wars.Buildings.Domain;

public record ResourceCollection
{
    public required int Clay { get; init; }
    public required int Iron { get; init; }
    public required int Wood { get; init; }
}
