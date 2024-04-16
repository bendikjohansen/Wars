using Wars.Common;

namespace Wars.Villages.Domain;

public record VillageCreatedEvent(Guid VillageId) : BaseDomainEvent;
