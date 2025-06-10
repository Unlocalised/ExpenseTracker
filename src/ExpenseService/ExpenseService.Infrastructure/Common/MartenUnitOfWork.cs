using ExpenseService.Infrastructure.Account;
using ExpenseService.Infrastructure.Transaction;
using ExpenseTracker.Application.Account;
using ExpenseTracker.Application.Common;
using ExpenseTracker.Application.Transaction;
using Marten;

namespace ExpenseService.Infrastructure.Common;
public class MartenUnitOfWork(IDocumentSession documentSession) : IUnitOfWork
{
    public IAccountRepository Accounts => new MartenAccountRepository(documentSession);

    public ITransactionRepository Transactions => new MartenTransactionRepository(documentSession);

    public async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        await documentSession.SaveChangesAsync(cancellationToken);
    }

    public void Dispose() => documentSession.Dispose();
}
