using FluentValidation;

namespace ExpenseService.Application.Account.WithdrawAccount;

public class WithdrawAccountCommandValidator : AbstractValidator<WithdrawAccountCommand>
{
    public WithdrawAccountCommandValidator()
    {

        RuleFor(withdrawAccountCommand => withdrawAccountCommand.Amount).GreaterThan(0);
    }
}
