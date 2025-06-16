using ExpenseService.Domain.Account;
using Marten;

namespace ExpenseService.Infrastructure.Account;
public class MartenAccountRepository(IDocumentSession documentSession) : IAccountRepository
{
    public void Create(AccountAggregate aggregateRoot)
    {
        var events = aggregateRoot.DequeueUncommittedEvents();
        documentSession.Events.StartStream<AccountAggregate>(aggregateRoot.Id, events);
    }

    public async Task<AccountAggregate?> LoadAsync(Guid streamId, CancellationToken cancellationToken = default)
    {
        return await documentSession.Events.AggregateStreamAsync<AccountAggregate>(streamId, token: cancellationToken);
    }

    public async Task SaveAsync(AccountAggregate aggregate, long expectedVersion, CancellationToken cancellationToken = default)
    {
        var events = aggregate.DequeueUncommittedEvents();
        if (events.Length == 0) return;
        var stream = await documentSession.Events.FetchForWriting<AccountAggregate>(aggregate.Id, expectedVersion, cancellationToken);
        stream.AppendMany(events);
    }
}
