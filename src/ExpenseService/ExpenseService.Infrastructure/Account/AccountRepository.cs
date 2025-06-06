using ExpenseTracker.Application.Account;
using ExpenseTracker.Domain.Account;
using ExpenseTracker.Domain.Common;
using Marten;

namespace ExpenseService.Infrastructure.Account;
public class AccountRepository(IDocumentSession documentSession) : IAccountRepository
{
    public void StartStream(AccountAggregate aggregateRoot)
    {
        var events = aggregateRoot.DequeueUncommittedEvents();
        documentSession.Events.StartStream<AccountAggregate>(aggregateRoot.Id, events);
    }

    public void Persist(AccountAggregate aggregateRoot)
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

    public async Task<AccountAggregate?> AggregateStreamAsync(Guid streamId, CancellationToken cancellationToken = default)
    {
        return await documentSession.Events.AggregateStreamAsync<AccountAggregate>(streamId, token: cancellationToken);
    }
}
