using MediatR;
using Wars.Resources.UseCases;
using Wars.Villages.Contracts;

namespace Wars.Resources.Integrations;

internal class VillageCreatedEventHandler(IMediator mediator) : INotificationHandler<VillageCreatedIntegrationEvent>
{
    private readonly IMediator _mediator = mediator;

    public async Task Handle(VillageCreatedIntegrationEvent notification, CancellationToken ct)
    {
        var createCommand = new CreateResourcesCommand(notification.VillageId);
        await _mediator.Send(createCommand, ct);
    }
}
