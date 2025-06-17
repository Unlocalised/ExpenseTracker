using ExpenseService.Application.Account.DepositAccount;
using FluentValidation.TestHelper;

namespace ExpenseService.UnitTests.Application.Account.DepositAccount;

public class DepositAccountValidatorUnitTest
{
    private readonly DepositAccountCommandValidator _validator = new();

    [Theory]
    [InlineData(-1)]
    public void DepositAccountValidator_ShouldHaveValidationErrorWhenAmountNegative(decimal amount)
    {
        var model = new DepositAccountCommand(Guid.NewGuid(), 2, amount);
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Amount);
    }

    [Fact]
    public void DepositAccountValidator_ShouldNotHaveValidationErrorWhenAmountPositive()
    {
        var model = new DepositAccountCommand(Guid.NewGuid(), 2, 100m);
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(x => x.Amount);
    }
}