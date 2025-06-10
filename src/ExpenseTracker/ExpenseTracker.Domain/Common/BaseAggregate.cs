namespace ExpenseTracker.Domain.Common;

public abstract class BaseAggregate : BaseEntity
{
    [NonSerialized] 
    private readonly Queue<BaseEvent> _uncommittedEvents = new();

    public BaseEvent[] DequeueUncommittedEvents()
    {
        var dequeuedEvents = _uncommittedEvents.ToArray();

        _uncommittedEvents.Clear();

        return dequeuedEvents;
    }

    public void Enqueue(BaseEvent @event) => _uncommittedEvents.Enqueue(@event);

}
