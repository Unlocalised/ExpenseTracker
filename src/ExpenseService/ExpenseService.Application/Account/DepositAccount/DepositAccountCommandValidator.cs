using FluentValidation;

namespace ExpenseService.Application.Account.DepositAccount;

public class DepositAccountCommandValidator : AbstractValidator<DepositAccountCommand>
{
    public DepositAccountCommandValidator()
    {

        RuleFor(depositAccountCommand => depositAccountCommand.Amount).GreaterThan(0);
    }
}
