using ExpenseService.Infrastructure.Account;
using ExpenseService.Infrastructure.Transaction;
using ExpenseTracker.Application.Account;
using ExpenseTracker.Application.Common;
using ExpenseTracker.Application.Transaction;
using Marten;

namespace ExpenseService.Infrastructure.Common;
public class MartenUnitOfWork(IDocumentSession documentSession) : IUnitOfWork
{
    private IAccountRepository? _accountRepository;
    private ITransactionRepository? _transactionRepository;

    public IAccountRepository Accounts => _accountRepository ??= new MartenAccountRepository(documentSession);

    public ITransactionRepository Transactions => _transactionRepository ??= new MartenTransactionRepository(documentSession);

    public async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        await documentSession.SaveChangesAsync(cancellationToken);
    }

    public void Dispose() => documentSession.Dispose();
}
