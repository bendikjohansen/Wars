using MediatR;
using Microsoft.Extensions.Logging;
using Wars.Resources.Domain;
using Wars.Villages.Contracts;

namespace Wars.Resources.Features;

internal static class CreateResources
{
    internal class VillageCreatedEventHandler(
        IResourcesRepository repository,
        ILogger<VillageCreatedEventHandler> logger) : INotificationHandler<VillageCreatedIntegrationEvent>
    {
        private readonly IResourcesRepository _repository = repository;
        private readonly ILogger<VillageCreatedEventHandler> _logger = logger;

        public async Task Handle(VillageCreatedIntegrationEvent notification, CancellationToken ct)
        {
            var existingVillage = await _repository.GetAsync(notification.VillageId, ct);
            if (existingVillage is not null)
            {
                _logger.LogWarning("Resources already exists for village with ID {villageId}.", notification.VillageId);
            }

            var resources = new Village
            {
                Id = notification.VillageId
            };
            _repository.Add(resources);
            await _repository.SaveChangesAsync(ct);
        }
    }
}
