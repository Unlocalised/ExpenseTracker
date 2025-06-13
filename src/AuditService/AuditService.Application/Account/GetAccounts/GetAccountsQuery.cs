using ExpenseTracker.Application.Account;
using ExpenseTracker.Domain.Account;

namespace AuditService.Application.Account.GetAccounts;

public record GetAccountsQuery;

public class GetAccountsQueryHandler
{
    public static async Task<IReadOnlyList<AccountReadModel>> Handle(GetAccountsQuery request, IAccountQueryRepository accountQueryRepository, CancellationToken cancellationToken)
    {
        return await accountQueryRepository.GetAccounts(cancellationToken);
    }
}
