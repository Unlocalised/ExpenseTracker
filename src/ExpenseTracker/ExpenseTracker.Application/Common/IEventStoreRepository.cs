using ExpenseTracker.Domain.Common;

namespace ExpenseTracker.Application.Common;

public interface IEventStoreRepository<TA> where TA : BaseAggregate
{
    Task<TA?> LoadAsync(Guid streamId, CancellationToken cancellationToken = default);

    Task SaveAsync(TA aggregate, long expectedVersion, CancellationToken cancellationToken = default);

    void Create(TA aggregateRoot);
}