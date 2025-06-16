
using ExpenseService.Api.Models.Accounts;
using ExpenseService.Application.Account.CreateAccount;
using ExpenseService.Application.Account.DeleteAccount;
using ExpenseService.Application.Account.DepositAccount;
using ExpenseService.Application.Account.UpdateAccount;
using ExpenseService.Application.Account.WithdrawAccount;
using ExpenseService.Application.Models.Accounts;
using ExpenseTracker.Api.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseService.Api.Controllers;

public class AccountsController : ApiControllerBase
{
    [HttpPost]
    public async Task<ActionResult<AccountCommandResult>> Create(CreateAccountCommand command, CancellationToken cancellationToken)
    {
        return await MessageBus.InvokeAsync<AccountCommandResult>(command, cancellationToken);
    }

    [HttpPut("{id}/deposit")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<AccountCommandResult>> Deposit(Guid id, [FromBody] DepositAccountRequest depositAccountRequest, CancellationToken cancellationToken)
    {
        return await MessageBus.InvokeAsync<AccountCommandResult>(new DepositAccountCommand(id, depositAccountRequest.ExpectedVersion, depositAccountRequest.Amount), cancellationToken);
    }

    [HttpPut("{id}/withdraw")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<AccountCommandResult>> Withdraw(Guid id, [FromBody] WithdrawAccountRequest withdrawAccountRequest, CancellationToken cancellationToken)
    {
        return await MessageBus.InvokeAsync<AccountCommandResult>(new WithdrawAccountCommand(id, withdrawAccountRequest.ExpectedVersion, withdrawAccountRequest.Amount), cancellationToken);
    }


    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<AccountCommandResult>> Update(Guid id, [FromBody] UpdateAccountRequest updateAccountRequest, CancellationToken cancellationToken)
    {
        return await MessageBus.InvokeAsync<AccountCommandResult>(new UpdateAccountCommand(id, updateAccountRequest.ExpectedVersion, updateAccountRequest.Name, updateAccountRequest.Number, updateAccountRequest.BankName, updateAccountRequest.BankPhone, updateAccountRequest.BankAddress, updateAccountRequest.Enabled), cancellationToken);
    }


    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> Delete(Guid id, long expectedVersion, CancellationToken cancellationToken)
    {

        await MessageBus.InvokeAsync(new DeleteAccountCommand(id, expectedVersion), cancellationToken);

        return NoContent();
    }
}