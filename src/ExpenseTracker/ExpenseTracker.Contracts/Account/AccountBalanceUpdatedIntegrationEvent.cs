namespace ExpenseTracker.Contracts.Account;
public record AccountBalanceUpdatedIntegrationEvent
{
    public Guid Id { get; set; }

    public decimal Balance { get; set; }

    public DateTime UpdatedAt { get; set; }

    public long ExpectedVersion { get; set; }

    public AccountBalanceUpdatedIntegrationEvent() { }
}