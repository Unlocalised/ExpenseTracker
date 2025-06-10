
using ExpenseService.Api.Models.Accounts;
using ExpenseService.Application.Account.CreateAccount;
using ExpenseService.Application.Account.DeleteAccount;
using ExpenseService.Application.Account.DepositAccount;
using ExpenseService.Application.Account.UpdateAccount;
using ExpenseService.Application.Account.WithdrawAccount;
using ExpenseTracker.Api.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseService.Api.Controllers;

public class AccountsController : ApiControllerBase
{
    [HttpPost]
    public async Task<ActionResult<Guid>> Create(CreateAccountCommand command, CancellationToken cancellationToken)
    {
        return await Mediator.Send(command, cancellationToken);
    }

    [HttpPut("{id}/deposit")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<Guid>> Deposit(Guid id, [FromBody] DepositAccountRequest depositAccountRequest, CancellationToken cancellationToken)
    {
        return await Mediator.Send(new DepositAccountCommand(id, depositAccountRequest.CurrentVersion, depositAccountRequest.Amount), cancellationToken);
    }

    [HttpPut("{id}/withdraw")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<Guid>> Withdraw(Guid id, [FromBody] WithdrawAccountRequest withdrawAccountRequest, CancellationToken cancellationToken)
    {
        return await Mediator.Send(new WithdrawAccountCommand(id, withdrawAccountRequest.CurrentVersion, withdrawAccountRequest.Amount), cancellationToken);
    }


    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateAccountRequest updateAccountRequest, CancellationToken cancellationToken)
    {
        await Mediator.Send(new UpdateAccountCommand(id, updateAccountRequest.CurrentVersion, updateAccountRequest.Name, updateAccountRequest.Number, updateAccountRequest.BankName, updateAccountRequest.BankPhone, updateAccountRequest.BankAddress, updateAccountRequest.Enabled), cancellationToken);
        return NoContent();
    }


    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> Delete(Guid id, long currentVersion, CancellationToken cancellationToken)
    {

        await Mediator.Send(new DeleteAccountCommand(id, currentVersion), cancellationToken);

        return NoContent();
    }
}