using AuditService.Application.Common;
using ExpenseTracker.Contracts.Account;
using Marten;

namespace AuditService.Infrastructure.Account.IntegrationHandlers;
public class AccountIntegrationEventHandler
{
    public static async Task Handle(AccountCreatedIntegrationEvent message, IDocumentSession session, ICacheService cacheService, CancellationToken cancellationToken)
    {
        var key = CacheKeyBuilder.ForAccounts();
        await cacheService.RemoveAsync(key, cancellationToken);
        session.Events.StartStream(message.Id, message);
        await session.SaveChangesAsync();
    }

    public static async Task Handle(AccountBalanceUpdatedIntegrationEvent message, IDocumentSession session, ICacheService cacheService, CancellationToken cancellationToken)
    {
        string[] keys = [CacheKeyBuilder.ForAccounts(), CacheKeyBuilder.ForAccount(message.Id)];
        await AppendEventAndInvalidateCache(message.Id, message, session, cacheService, keys, cancellationToken);
    }

    public static async Task Handle(AccountDeletedIntegrationEvent message, IDocumentSession session, ICacheService cacheService, CancellationToken cancellationToken)
    {
        string[] keys = [CacheKeyBuilder.ForAccounts(), CacheKeyBuilder.ForAccount(message.Id)];
        await AppendEventAndInvalidateCache(message.Id, message, session, cacheService, keys, cancellationToken);
    }

    public static async Task Handle(AccountUpdatedIntegrationEvent message, IDocumentSession session, ICacheService cacheService, CancellationToken cancellationToken)
    {
        string[] keys = [CacheKeyBuilder.ForAccounts(), CacheKeyBuilder.ForAccount(message.Id)];
        await AppendEventAndInvalidateCache(message.Id, message, session, cacheService, keys, cancellationToken);
    }

    private static async Task AppendEventAndInvalidateCache<T>(
    Guid streamId,
    T @event,
    IDocumentSession session,
    ICacheService cache,
    string[] cacheKeys,
    CancellationToken ct) where T : class
    {
        await session.Events.AppendExclusive(streamId, @event);

        foreach (var key in cacheKeys)
            await cache.RemoveAsync(key, ct);

        await session.SaveChangesAsync(ct);
    }
}
