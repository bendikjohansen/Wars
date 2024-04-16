using MediatR;

namespace Wars.Common;

public interface IDomainEventDispatcher
{
    Task DispatchAndClearEventsAsync(IEnumerable<IHaveDomainEvents> entitiesWithEvents, CancellationToken ct = default);
}

public class MediatrDomainEventDispatcher(IMediator mediator) : IDomainEventDispatcher
{
    private readonly IMediator _mediator = mediator;

    public async Task DispatchAndClearEventsAsync(IEnumerable<IHaveDomainEvents> entitiesWithEvents, CancellationToken ct = default)
    {
        foreach (var entity in entitiesWithEvents)
        {
            var domainEvents = entity.DomainEvents;
            entity.ClearEvents();
            foreach (var domainEvent in domainEvents)
            {
                await _mediator.Publish(domainEvent, ct);
            }
        }
    }
}
