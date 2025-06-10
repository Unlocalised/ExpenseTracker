namespace ExpenseService.Api.Models.Accounts;

public class WithdrawAccountRequest
{
    public long CurrentVersion { get; set; }

    public decimal Amount { get; set; }
}
