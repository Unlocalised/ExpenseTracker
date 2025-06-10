using ExpenseTracker.Domain.Common;

namespace ExpenseTracker.Application.Common;

public interface IEventStoreRepository<TA> where TA : BaseAggregate
{
    Task<TA?> LoadAsync(Guid streamId, CancellationToken cancellationToken = default);

    void Append(Guid streamId, long expectedVersion, params BaseEvent[] @events);

    Task SaveAsync(TA aggregate, long currentVersion, CancellationToken cancellationToken = default);

    void Create(TA aggregateRoot);
}