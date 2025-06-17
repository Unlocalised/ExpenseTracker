using ExpenseTracker.Application.Common.Exceptions;
using AuditService.Domain.Account;
using Marten;

namespace AuditService.Infrastructure.Account;
public class AccountQueryRepository(IDocumentSession documentSession) : IAccountQueryRepository
{
    public async Task<AccountReadModel> GetAccountByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await documentSession.LoadAsync<AccountReadModel>(id, cancellationToken)
            ?? throw new NotFoundException(nameof(AccountReadModel), id);
    }

    public async Task<IReadOnlyList<AccountReadModel>> GetAccountsAsync(CancellationToken cancellationToken = default)
    {
        return await documentSession.Query<AccountReadModel>()
                       .ToListAsync(cancellationToken);
    }
}
