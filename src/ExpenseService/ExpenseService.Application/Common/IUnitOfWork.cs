using ExpenseService.Domain.Account;
using ExpenseService.Domain.Transaction;

namespace ExpenseService.Application.Common;
public interface IUnitOfWork : IDisposable
{
    IAccountRepository Accounts { get; }
    ITransactionRepository Transactions { get; }

    Task CommitAsync(CancellationToken cancellationToken = default);
    Task PublishAsync<T>(T message);
}