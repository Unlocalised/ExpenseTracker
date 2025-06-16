using AuditService.Domain.Account;

namespace AuditService.Application.Account.GetAccountById;

public record GetAccountByIdQuery(Guid Id);

public class GetAccountByIdQueryHandler
{
    public static async Task<AccountReadModel> Handle(GetAccountByIdQuery request, IAccountQueryRepository accountQueryRepository, CancellationToken cancellationToken)
    {
        var account = await accountQueryRepository.GetAccountById(request.Id, cancellationToken);
        return account.DeletedAt.HasValue ? throw new InvalidOperationException("Account already deleted") : account;
    }
}
