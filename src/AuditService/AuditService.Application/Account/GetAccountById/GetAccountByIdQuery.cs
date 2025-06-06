using MediatR;
using ExpenseTracker.Domain.Account;
using ExpenseTracker.Application.Account;

namespace AuditService.Application.Account.GetAccountById;

public record GetAccountByIdQuery(Guid Id) : IRequest<AccountReadModel>;

public class GetAccountByIdQueryHandler(IAccountQueryRepository accountQueryRepository) : IRequestHandler<GetAccountByIdQuery, AccountReadModel>
{
    public async Task<AccountReadModel> Handle(GetAccountByIdQuery request, CancellationToken cancellationToken)
    {
        var account = await accountQueryRepository.GetAccountById(request.Id, cancellationToken);
        if (account.DeletedAt.HasValue)
            throw new InvalidOperationException("Account already deleted");
        return account;
    }
}
