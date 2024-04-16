using MediatR;
using Wars.Villages.Contracts;
using Wars.Villages.Domain;

namespace Wars.Villages.Integrations;

internal class VillageCreatedIntegrationEventDispatcherHandler(IMediator mediator) : INotificationHandler<VillageCreatedEvent>
{
    private readonly IMediator _mediator = mediator;

    public async Task Handle(VillageCreatedEvent domainEvent, CancellationToken ct)
    {
        var integrationEvent = new VillageCreatedIntegrationEvent(domainEvent.VillageId.ToString());
        await _mediator.Publish(integrationEvent, ct);
    }
}
