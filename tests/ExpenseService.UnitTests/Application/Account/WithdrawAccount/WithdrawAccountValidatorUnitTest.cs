using ExpenseService.Application.Account.WithdrawAccount;
using FluentValidation.TestHelper;

namespace ExpenseService.UnitTests.Application.Account.WithdrawAccount;

public class WithdrawAccountValidatorUnitTest
{
    private readonly WithdrawAccountCommandValidator _validator = new();

    [Theory]
    [InlineData(-1)]
    public void WithdrawAccountValidator_ShouldHaveValidationErrorWhenAmountNegative(decimal amount)
    {
        var model = new WithdrawAccountCommand(Guid.NewGuid(), 2, amount);
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Amount);
    }

    [Fact]
    public void WithdrawAccountValidator_ShouldNotHaveValidationErrorWhenAmountPositive()
    {
        var model = new WithdrawAccountCommand(Guid.NewGuid(), 2, 100m);
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(x => x.Amount);
    }
}