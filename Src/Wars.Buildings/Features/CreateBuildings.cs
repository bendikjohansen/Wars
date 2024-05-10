using MediatR;
using Microsoft.Extensions.Logging;
using Wars.Buildings.Domain;
using Wars.Villages.Contracts;

namespace Wars.Buildings.Features;

internal static class CreateBuildings
{
    internal class VillageCreatedIntegrationEventHandler(
        IBuildingsRepository buildingsRepository,
        ILogger<VillageCreatedIntegrationEventHandler> logger) : INotificationHandler<VillageCreatedIntegrationEvent>
    {
        private readonly IBuildingsRepository _buildingsRepository = buildingsRepository;
        private readonly ILogger<VillageCreatedIntegrationEventHandler> _logger = logger;

        public async Task Handle(VillageCreatedIntegrationEvent notification, CancellationToken ct)
        {
            var existingVillage = await _buildingsRepository.GetAsync(notification.VillageId, ct);
            if (existingVillage is not null)
            {
                _logger.LogWarning("{Module} already exists for village with ID {VillageId}.", "Buildings", notification.VillageId);
                return;
            }

            var village = Village.CreateFrom(notification.VillageId);
            _buildingsRepository.Add(village);
            await _buildingsRepository.SaveChangesAsync(ct);
        }
    }
}
