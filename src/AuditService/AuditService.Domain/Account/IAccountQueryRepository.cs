namespace AuditService.Domain.Account;

public interface IAccountQueryRepository
{
    Task<AccountReadModel> GetAccountByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<AccountReadModel>> GetAccountsAsync(CancellationToken cancellationToken = default);
}