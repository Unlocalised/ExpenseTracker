using ExpenseTracker.Application.Common.Exceptions;
using ExpenseTracker.Application.Transaction;
using ExpenseTracker.Domain.Transaction;
using ExpenseTracker.Domain.Common;
using Marten.Exceptions;
using Marten;

namespace ExpenseService.Infrastructure.Transaction;
public class MartenTransactionRepository(IDocumentSession documentSession) : ITransactionRepository
{
    public void Create(TransactionAggregate aggregateRoot)
    {
        var events = aggregateRoot.DequeueUncommittedEvents();
        documentSession.Events.StartStream<TransactionAggregate>(aggregateRoot.Id, events);
    }

    public void Append(Guid streamId, long expectedVersion, params BaseEvent[] events)
    {
        if (events.Length == 0) return;
        documentSession.Events.Append(streamId, expectedVersion, events);
    }

    public async Task SaveAsync(TransactionAggregate aggregate, long currentVersion, CancellationToken cancellationToken = default)
    {
        var streamState = await documentSession.Events.FetchStreamStateAsync(aggregate.Id, cancellationToken) ?? throw new NotFoundException("Transaction not found");
        if (streamState.Version != currentVersion)
            throw new ConcurrencyException(typeof(TransactionAggregate), aggregate.Id);

        var events = aggregate.DequeueUncommittedEvents();
        Append(aggregate.Id, currentVersion + events.Length, events);
    }

    public async Task<TransactionAggregate?> LoadAsync(Guid streamId, CancellationToken cancellationToken = default)
    {
        return await documentSession.Events.AggregateStreamAsync<TransactionAggregate>(streamId, token: cancellationToken);
    }
}
