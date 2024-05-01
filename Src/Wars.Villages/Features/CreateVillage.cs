using FastEndpoints;
using MediatR;
using Microsoft.Extensions.Logging;
using Wars.Villages.Contracts;
using Wars.Villages.Models;

namespace Wars.Villages.Features;

internal static class CreateVillage
{
    internal class Endpoint(
        IVillagesRepository villagesRepository,
        ILogger<Endpoint> logger,
        IMediator mediator) : Endpoint<Endpoint.Request>
    {
        private readonly IVillagesRepository _villagesRepository = villagesRepository;
        private readonly ILogger<Endpoint> _logger = logger;
        private readonly IMediator _mediator = mediator;

        public override void Configure()
        {
            Post("create-village");
            Claims("UserId");
        }

        public override async Task HandleAsync(Request req, CancellationToken ct)
        {
            var userId = User.FindFirst("UserId")!.Value;

            var villages = await _villagesRepository.ListByUserAsync(userId, ct);

            if (villages.Any())
            {
                _logger.LogWarning("User {userId} has already created a village", userId);
                await SendErrorsAsync(cancellation: ct);
                return;
            }

            var newVillage = Village.Factory.Create(userId, req.Name);
            _villagesRepository.Add(newVillage);
            await _villagesRepository.SaveChangesAsync(ct);

            await _mediator.Publish(new VillageCreatedIntegrationEvent(newVillage.Id.ToString()), ct);
        }

        internal record Request(string Name);
    }
}
