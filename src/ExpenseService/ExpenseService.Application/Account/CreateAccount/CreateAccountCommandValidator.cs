using FluentValidation;

namespace ExpenseService.Application.Account.CreateAccount;

public class CreateAccountCommandValidator : AbstractValidator<CreateAccountCommand>
{
    public CreateAccountCommandValidator()
    {
        RuleFor(createAccountCommand => createAccountCommand.Name).NotEmpty().MaximumLength(255);

        RuleFor(createAccountCommand => createAccountCommand.BankName).MaximumLength(1500);

        RuleFor(createAccountCommand => createAccountCommand.OpeningBalance).GreaterThan(0).When(createAccountCommand => createAccountCommand.OpeningBalance.HasValue);
    }
}
