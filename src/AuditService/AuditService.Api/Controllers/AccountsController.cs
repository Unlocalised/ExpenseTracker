
using AuditService.Application.Account.GetAccountById;
using AuditService.Application.Account.GetAccounts;
using AuditService.Domain.Account;
using ExpenseTracker.Api.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace AuditService.Api.Controllers;

public class AccountsController : ApiControllerBase
{
    [HttpGet("{id}")]
    public async Task<ActionResult<AccountReadModel>> GetAccountById(Guid id, CancellationToken cancellationToken)
    {
        return await MessageBus.InvokeAsync<AccountReadModel>(new GetAccountByIdQuery(id), cancellationToken);
    }

    [HttpGet]
    public async Task<IReadOnlyList<AccountReadModel>> GetAccounts(CancellationToken cancellationToken)
    {
        return await MessageBus.InvokeAsync<IReadOnlyList<AccountReadModel>>(new GetAccountsQuery(), cancellationToken);
    }
}
