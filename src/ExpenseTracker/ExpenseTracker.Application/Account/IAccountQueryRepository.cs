using ExpenseTracker.Domain.Account;

namespace ExpenseTracker.Application.Account;

public interface IAccountQueryRepository
{
    Task<AccountReadModel> GetAccountById(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<AccountReadModel>> GetAccounts(CancellationToken cancellationToken = default);
}