using AuditService.Application.Common;
using AuditService.Domain.Account;

namespace AuditService.Application.Account.GetAccounts;

public record GetAccountsQuery;

public class GetAccountsQueryHandler
{
    public static async Task<IReadOnlyList<AccountReadModel>> Handle(
        GetAccountsQuery request,
        IAccountQueryRepository accountQueryRepository,
        ICacheService cacheService,
        CancellationToken cancellationToken)
    {
        var key = CacheKeyBuilder.ForAccounts();
        var cached = await cacheService.GetAsync<IReadOnlyList<AccountReadModel>>(key, cancellationToken);
        if (cached != null) return cached;
        var accounts = await accountQueryRepository.GetAccountsAsync(cancellationToken);
        await cacheService.SetAsync(key, accounts, TimeSpan.Zero, cancellationToken);

        return accounts;
    }
}
