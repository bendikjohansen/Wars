using MediatR;

namespace Wars.Villages.Contracts;

public record VillageCreatedIntegrationEvent(string VillageId) : INotification;
