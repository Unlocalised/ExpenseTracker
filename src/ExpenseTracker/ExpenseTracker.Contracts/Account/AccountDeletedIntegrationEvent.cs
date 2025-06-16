namespace ExpenseTracker.Contracts.Account;
public record AccountDeletedIntegrationEvent
{
    public Guid Id { get; set; }

    public DateTime DeletedAt { get; set; }

    public AccountDeletedIntegrationEvent() { }
}