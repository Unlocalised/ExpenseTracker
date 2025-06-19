using ExpenseService.Application.Account.UpdateAccount;
using FluentValidation.TestHelper;

namespace ExpenseService.UnitTests.Application.Account.UpdateAccount;

public class UpdateAccountValidatorTests
{
    private readonly UpdateAccountCommandValidator _validator = new();

    [Fact]
    public void UpdateAccountValidator_ShouldHaveValidationErrorWhenNameExceedsMaxLength()
    {
        var model = new UpdateAccountCommand(Guid.NewGuid(), default(int), new string('a', 256), string.Empty, string.Empty, string.Empty, string.Empty, false);
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void UpdateAccountValidator_ShouldNotHaveValidationErrorWhenValidName()
    {
        var model = new UpdateAccountCommand(Guid.NewGuid(), default(int), "Valid Name", string.Empty, string.Empty, string.Empty, string.Empty, false);
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void UpdateAccountValidator_ShouldHaveValidationErrorWhenBankNameExceedsMaxLength()
    {
        var model = new UpdateAccountCommand(Guid.NewGuid(), default(int), string.Empty, string.Empty, new string('a', 1501), string.Empty, string.Empty, false);
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.BankName);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("Valid Bank Name")]
    public void UpdateAccountValidator_ShouldNotHaveValidationErrorWhenBankNameValid(string? bankName)
    {
        var model = new UpdateAccountCommand(Guid.NewGuid(), default(int), string.Empty, string.Empty, bankName, string.Empty, string.Empty, false);
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(x => x.BankName);
    }
}