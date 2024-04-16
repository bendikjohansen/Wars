using System.ComponentModel.DataAnnotations.Schema;
using Wars.Common;

namespace Wars.Villages.Domain;

internal record Village : IHaveDomainEvents
{
    public Guid Id { get; } = Guid.NewGuid();
    public string OwnerId { get; private set; } = string.Empty;
    public string Name { get; private set; } = string.Empty;

    private readonly List<BaseDomainEvent> _domainEvents = [];
    [NotMapped] public IEnumerable<BaseDomainEvent> DomainEvents => _domainEvents.ToArray();

    public void ClearEvents() => _domainEvents.Clear();

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

            village._domainEvents.Add(new VillageCreatedEvent(village.Id));
            return village;
        }
    }
}
