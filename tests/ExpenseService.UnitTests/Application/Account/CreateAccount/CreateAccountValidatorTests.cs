using ExpenseService.Application.Account.CreateAccount;
using FluentValidation.TestHelper;

namespace ExpenseService.UnitTests.Application.Account.CreateAccount;

public class CreateAccountValidatorTests
{
    private readonly CreateAccountCommandValidator _validator = new();

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void CreateAccountValidator_ShouldHaveValidationErrorWhenNameEmpty(string name)
    {
        var model = new CreateAccountCommand { Name = name };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void CreateAccountValidator_ShouldHaveValidationErrorWhenNameExceedsMaxLength()
    {
        var model = new CreateAccountCommand { Name = new string('a', 256) };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void CreateAccountValidator_ShouldNotHaveValidationErrorWhenValidName()
    {
        var model = new CreateAccountCommand { Name = "Valid Name" };
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void CreateAccountValidator_ShouldHaveValidationErrorWhenBankNameExceedsMaxLength()
    {
        var model = new CreateAccountCommand { Name = string.Empty, BankName = new string('a', 1501) };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.BankName);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("Valid Bank Name")]
    public void CreateAccountValidator_ShouldNotHaveValidationErrorWhenBankNameValid(string? bankName)
    {
        var model = new CreateAccountCommand { Name = string.Empty, BankName = bankName };
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(x => x.BankName);
    }

    [Theory]
    [InlineData(-1)]
    public void CreateAccountValidator_ShouldHaveValidationErrorWhenOpeningBalanceNegative(decimal balance)
    {
        var model = new CreateAccountCommand { Name = string.Empty, OpeningBalance = balance };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.OpeningBalance);
    }

    [Fact]
    public void CreateAccountValidator_ShouldNotHaveValidationErrorWhenOpeningBalanceNull()
    {
        var model = new CreateAccountCommand { Name = string.Empty, OpeningBalance = null };
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(x => x.OpeningBalance);
    }

    [Fact]
    public void CreateAccountValidator_ShouldNotHaveValidationErrorWhenOpeningBalancePositive()
    {
        var model = new CreateAccountCommand {Name = string.Empty, OpeningBalance = 100m };
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(x => x.OpeningBalance);
    }
}