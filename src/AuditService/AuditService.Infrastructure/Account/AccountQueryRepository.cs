using AuditService.Domain.Account;
using ExpenseTracker.Application.Common.Exceptions;
using Marten;

namespace AuditService.Infrastructure.Account;
public class AccountQueryRepository(IDocumentSession documentSession) : IAccountQueryRepository
{
    public async Task<AccountReadModel> GetAccountById(Guid id, CancellationToken cancellationToken = default)
    {
        return await documentSession.LoadAsync<AccountReadModel>(id, cancellationToken)
            ?? throw new NotFoundException(nameof(AccountReadModel), id);
    }

    public async Task<IReadOnlyList<AccountReadModel>> GetAccounts(CancellationToken cancellationToken = default)
    {
        return await documentSession.Query<AccountReadModel>()
                       .Where(x => x.Enabled)
                       .ToListAsync(cancellationToken);
    }
}
