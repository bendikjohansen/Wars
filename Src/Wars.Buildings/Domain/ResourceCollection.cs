namespace Wars.Buildings.Domain;

public record ResourceCollection
{
    public required int Clay { get; init; }
    public required int Iron { get; init; }
    public required int Wood { get; init; }

    private ResourceCollection() {}

    public static ResourceCollection CreateFrom(int clay, int iron, int wood) =>
        new() { Clay = clay, Iron = iron, Wood = wood };

    public static ResourceCollection Empty => CreateFrom(0, 0, 0);
}
