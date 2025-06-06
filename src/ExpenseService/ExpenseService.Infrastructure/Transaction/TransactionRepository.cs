using ExpenseTracker.Application.Transaction;
using ExpenseTracker.Domain.Transaction;
using ExpenseTracker.Domain.Common;
using Marten;

namespace ExpenseService.Infrastructure.Transaction;
public class TransactionRepository(IDocumentSession documentSession) : ITransactionRepository
{
    public void StartStream(TransactionAggregate aggregateRoot)
    {
        var events = aggregateRoot.DequeueUncommittedEvents();
        documentSession.Events.StartStream<TransactionAggregate>(aggregateRoot.Id, events);
    }

    public void Persist(TransactionAggregate aggregateRoot)
    {
        var events = aggregateRoot.DequeueUncommittedEvents();
        Persist(aggregateRoot.Id, aggregateRoot.Version + 1, events);
    }

    public void Persist(Guid streamId, long expectedVersion, params BaseEvent[] events)
    {
        if (events.Length == 0) return;
        documentSession.Events.Append(streamId, expectedVersion, events);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await documentSession.SaveChangesAsync(cancellationToken);
    }

    public Task<TransactionAggregate> AggregateStreamAsync(Guid streamId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
