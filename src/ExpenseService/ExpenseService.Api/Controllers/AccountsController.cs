
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
    public async Task<ActionResult<Guid>> Deposit(Guid id, DepositAccountCommand command, CancellationToken cancellationToken)
    {
        if (id != command.AccountId)
        {
            return BadRequest();
        }

        return await Mediator.Send(command, cancellationToken);
    }

    [HttpPut("{id}/withdraw")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<Guid>> Withdraw(Guid id, WithdrawAccountCommand command, CancellationToken cancellationToken)
    {
        if (id != command.AccountId)
        {
            return BadRequest();
        }

        return await Mediator.Send(command, cancellationToken);
    }


    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> Update(Guid id, UpdateAccountCommand command, CancellationToken cancellationToken)
    {
        if (id != command.Id)
        {
            return BadRequest();
        }

        await Mediator.Send(command, cancellationToken);

        return NoContent();
    }


    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> Delete(Guid id, long version, CancellationToken cancellationToken)
    {

        await Mediator.Send(new DeleteAccountCommand(id, version), cancellationToken);

        return NoContent();
    }
}