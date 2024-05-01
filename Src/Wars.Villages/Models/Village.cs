namespace Wars.Villages.Models;

internal record Village
{
    public Guid Id { get; } = Guid.NewGuid();
    public string OwnerId { get; private set; } = string.Empty;
    public string Name { get; private set; } = string.Empty;

    internal static class Factory
    {
        public static Village Create(string ownerId, string name)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(ownerId);
            ArgumentException.ThrowIfNullOrWhiteSpace(name);

            var village = new Village
            {
                OwnerId = ownerId,
                Name = name
            };

            return village;
        }
    }
}
