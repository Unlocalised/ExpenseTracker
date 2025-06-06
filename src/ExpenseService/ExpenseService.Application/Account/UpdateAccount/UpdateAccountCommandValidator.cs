using FluentValidation;

namespace ExpenseService.Application.Account.UpdateAccount;

public class UpdateAccountCommandValidator : AbstractValidator<UpdateAccountCommand>
{
    public UpdateAccountCommandValidator()
    {
        RuleFor(anamnesis => anamnesis.Name).NotEmpty().MaximumLength(255);

        RuleFor(anamnesis => anamnesis.BankName).MaximumLength(1500);
    }
}
