using MediatR;
using Microsoft.Extensions.Logging;
using Wars.Resources.Domain;
using Wars.Villages.Contracts;

namespace Wars.Resources.Features;

internal static class CreateResources
{
    internal class VillageCreatedIntegrationEventHandler(
        IResourcesRepository repository,
        ILogger<VillageCreatedIntegrationEventHandler> logger,
        TimeProvider time) : INotificationHandler<VillageCreatedIntegrationEvent>
    {
        private readonly IResourcesRepository _repository = repository;
        private readonly ILogger<VillageCreatedIntegrationEventHandler> _logger = logger;
        private readonly TimeProvider _time = time;

        public async Task Handle(VillageCreatedIntegrationEvent notification, CancellationToken ct)
        {
            var existingVillage = await _repository.GetAsync(notification.VillageId, ct);
            if (existingVillage is not null)
            {
                _logger.LogWarning("{Module} already exists for village with ID {VillageId}.", "Resources", notification.VillageId);
                return;
            }

            var village = new Village(_time.GetUtcNow())
            {
                Id = notification.VillageId
            };
            _repository.Add(village);
            await _repository.SaveChangesAsync(ct);
        }
    }
}
