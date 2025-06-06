using MediatR;
using ExpenseTracker.Domain.Account;
using ExpenseTracker.Application.Account;

namespace AuditService.Application.Account.GetAccounts;

public record GetAccountsQuery : IRequest<IReadOnlyList<AccountReadModel>>;

public class GetAccountsQueryHandler(IAccountQueryRepository accountQueryRepository) : IRequestHandler<GetAccountsQuery, IReadOnlyList<AccountReadModel>>
{
    public async Task<IReadOnlyList<AccountReadModel>> Handle(GetAccountsQuery request, CancellationToken cancellationToken)
    {
        return await accountQueryRepository.GetAccounts(cancellationToken);
    }
}
