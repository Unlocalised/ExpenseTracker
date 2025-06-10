namespace ExpenseService.Api.Models.Accounts;

public class WithdrawAccountRequest
{
    public long ExpectedVersion { get; set; }

    public decimal Amount { get; set; }
}
