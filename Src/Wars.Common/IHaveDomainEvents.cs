namespace Wars.Common;

public interface IHaveDomainEvents
{
    IEnumerable<BaseDomainEvent> DomainEvents { get; }
    void ClearEvents();
}
