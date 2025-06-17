using AuditService.Domain.Account;

namespace AuditService.Application.Account.GetAccountById;

public record GetAccountByIdQuery(Guid Id);

public class GetAccountByIdQueryHandler
{
    public static async Task<AccountReadModel> Handle(GetAccountByIdQuery request, IAccountQueryRepository accountQueryRepository, CancellationToken cancellationToken)
    {
        return await accountQueryRepository.GetAccountByIdAsync(request.Id, cancellationToken);
    }
}
