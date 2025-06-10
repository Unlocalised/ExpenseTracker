namespace ExpenseService.Application.Models.Accounts;

public record AccountCommandResult
{
    public long NewVersion { get; set; }
    public Guid AccountId { get; set; }
}
