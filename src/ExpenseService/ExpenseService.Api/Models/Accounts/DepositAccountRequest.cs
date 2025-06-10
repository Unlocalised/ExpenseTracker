namespace ExpenseService.Api.Models.Accounts;

public class DepositAccountRequest
{
    public long ExpectedVersion { get; set; }

    public decimal Amount { get; set; }
}
