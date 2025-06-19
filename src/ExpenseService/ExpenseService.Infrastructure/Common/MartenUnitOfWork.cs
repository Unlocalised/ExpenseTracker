using ExpenseService.Application.Common;
using ExpenseService.Domain.Account;
using ExpenseService.Domain.Transaction;
using ExpenseService.Infrastructure.Account;
using ExpenseService.Infrastructure.Transaction;
using Marten;
using Wolverine;

namespace ExpenseService.Infrastructure.Common;
public class MartenUnitOfWork(IDocumentSession documentSession, IMessageContext messageContext) : IUnitOfWork
{
    private IAccountRepository? _accountRepository;
    private ITransactionRepository? _transactionRepository;

    public IAccountRepository Accounts => _accountRepository ??= new MartenAccountRepository(documentSession);

    public ITransactionRepository Transactions => _transactionRepository ??= new MartenTransactionRepository(documentSession);

    public async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        await documentSession.SaveChangesAsync(cancellationToken);
    }
    public async Task PublishAsync<T>(T message) => await messageContext.PublishAsync(message);
}
