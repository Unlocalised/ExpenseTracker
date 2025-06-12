using FluentValidation;

namespace ExpenseService.Application.Account.UpdateAccount;

public class UpdateAccountCommandValidator : AbstractValidator<UpdateAccountCommand>
{
    public UpdateAccountCommandValidator()
    {
        RuleFor(updateAccountCommand => updateAccountCommand.Name).NotEmpty().MaximumLength(255);

        RuleFor(updateAccountCommand => updateAccountCommand.BankName).MaximumLength(1500);
    }
}
