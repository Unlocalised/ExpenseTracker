using FluentValidation;

namespace ExpenseService.Application.Account.CreateAccount;

public class CreateAccountCommandValidator : AbstractValidator<CreateAccountCommand>
{
    public CreateAccountCommandValidator()
    {
        RuleFor(createAccountCommand => createAccountCommand.Name).MaximumLength(255);

        RuleFor(createAccountCommand => createAccountCommand.BankName).MaximumLength(1500);
    }
}
