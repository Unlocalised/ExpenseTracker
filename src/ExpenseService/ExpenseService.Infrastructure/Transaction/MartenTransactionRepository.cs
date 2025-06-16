using ExpenseService.Domain.Transaction;
using Marten;

namespace ExpenseService.Infrastructure.Transaction;
public class MartenTransactionRepository(IDocumentSession documentSession) : ITransactionRepository
{
    public void Create(TransactionAggregate aggregateRoot)
    {
        var events = aggregateRoot.DequeueUncommittedEvents();
        documentSession.Events.StartStream<TransactionAggregate>(aggregateRoot.Id, events);
    }

    public async Task SaveAsync(TransactionAggregate aggregate, long expectedVersion, CancellationToken cancellationToken = default)
    {
        var events = aggregate.DequeueUncommittedEvents();
        if (events.Length == 0) return;
        var stream = await documentSession.Events.FetchForWriting<TransactionAggregate>(aggregate.Id, expectedVersion, cancellationToken);
        stream.AppendMany(events);
    }

    public async Task<TransactionAggregate?> LoadAsync(Guid streamId, CancellationToken cancellationToken = default)
    {
        return await documentSession.Events.AggregateStreamAsync<TransactionAggregate>(streamId, token: cancellationToken);
    }
}
