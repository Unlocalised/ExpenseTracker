using ExpenseTracker.Domain.Common;

namespace ExpenseTracker.Application.Common;

public interface IEventStoreRepository<TA> where TA : BaseAggregate
{
    Task<TA?> AggregateStreamAsync(Guid streamId, CancellationToken cancellationToken = default);
    
    void Persist(TA aggregateRoot);

    void Persist(Guid streamId, long expectedVersion, params BaseEvent[] @events);

    void StartStream(TA aggregateRoot);

    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}