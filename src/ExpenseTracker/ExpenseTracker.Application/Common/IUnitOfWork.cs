using ExpenseTracker.Application.Account;
using ExpenseTracker.Application.Transaction;

namespace ExpenseTracker.Application.Common;
public interface IUnitOfWork : IDisposable
{
    IAccountRepository Accounts { get; }
    ITransactionRepository Transactions { get; }

    Task CommitAsync(CancellationToken cancellationToken = default);
}