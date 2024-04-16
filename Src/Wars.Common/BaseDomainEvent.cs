using MediatR;

namespace Wars.Common;

public record BaseDomainEvent : INotification
{
    public DateTimeOffset OccurredAt { get; } = DateTimeOffset.Now;
}
