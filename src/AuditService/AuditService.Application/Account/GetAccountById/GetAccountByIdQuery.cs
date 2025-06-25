using AuditService.Application.Common;
using AuditService.Domain.Account;

namespace AuditService.Application.Account.GetAccountById;

public record GetAccountByIdQuery(Guid Id);

public class GetAccountByIdQueryHandler
{
    public static async Task<AccountReadModel> Handle(
        GetAccountByIdQuery request,
        IAccountQueryRepository accountQueryRepository,
        ICacheService cacheService,
        CancellationToken cancellationToken)
    {
        var key = CacheKeyBuilder.ForAccount(request.Id);
        var cached = await cacheService.GetAsync<AccountReadModel>(key, cancellationToken);
        if (cached != null) return cached;
        var accountReadModel = await accountQueryRepository.GetAccountByIdAsync(request.Id, cancellationToken);
        await cacheService.SetAsync(key, accountReadModel, TimeSpan.Zero, cancellationToken);

        return accountReadModel;
    }
}
