namespace ExpenseService.Api.Models.Accounts;

public class DepositAccountRequest
{
    public long CurrentVersion { get; set; }

    public decimal Amount { get; set; }
}
