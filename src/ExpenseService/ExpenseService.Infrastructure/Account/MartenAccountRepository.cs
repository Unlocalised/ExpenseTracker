using System.Threading;
using ExpenseTracker.Application.Account;
using ExpenseTracker.Application.Common.Exceptions;
using ExpenseTracker.Domain.Account;
using ExpenseTracker.Domain.Common;
using ExpenseTracker.Domain.Transaction;
using Marten;
using Marten.Exceptions;

namespace ExpenseService.Infrastructure.Account;
public class MartenAccountRepository(IDocumentSession documentSession) : IAccountRepository
{
    public void Create(AccountAggregate aggregateRoot)
    {
        var events = aggregateRoot.DequeueUncommittedEvents();
        documentSession.Events.StartStream<AccountAggregate>(aggregateRoot.Id, events);
    }

    public void Append(Guid streamId, long expectedVersion, params BaseEvent[] events)
    {
        if (events.Length == 0) return;
        documentSession.Events.Append(streamId, expectedVersion, events);
    }

    public async Task<AccountAggregate?> LoadAsync(Guid streamId, CancellationToken cancellationToken = default)
    {
        return await documentSession.Events.AggregateStreamAsync<AccountAggregate>(streamId, token: cancellationToken);
    }

    public async Task SaveAsync(AccountAggregate aggregate, long currentVersion, CancellationToken cancellationToken = default)
    {
        var streamState = await documentSession.Events.FetchStreamStateAsync(aggregate.Id, cancellationToken) ?? throw new NotFoundException("Account not found");
        if (streamState.Version != currentVersion)
            throw new ConcurrencyException(typeof(AccountAggregate), aggregate.Id);

        var events = aggregate.DequeueUncommittedEvents();
        Append(aggregate.Id, currentVersion + events.Length, events);
    }
}
