namespace ExpenseTracker.BuildingBlocks.Common;

public abstract class BaseAggregate : BaseEntity
{
    public long Version { get; set; } = 1;

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
