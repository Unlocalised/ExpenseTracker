namespace ExpenseService.Api.Models.Accounts;

public class UpdateAccountRequest
{
    public long CurrentVersion { get; set; }

    public string? Name { get; set; }

    public string? Number { get; set; }

    public string? BankName { get; set; }

    public string? BankPhone { get; set; }

    public string? BankAddress { get; set; }

    public bool? Enabled { get; set; }
}
