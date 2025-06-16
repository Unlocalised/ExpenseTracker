namespace ExpenseTracker.Contracts.Account;
public record AccountCreatedIntegrationEvent
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public decimal Balance { get; set; }

    public string? Number { get; set; }

    public string? BankName { get; set; }

    public string? BankPhone { get; set; }

    public string? BankAddress { get; set; }

    public DateTime? CreatedAt { get; set; }

    public long ExpectedVersion { get; set; }

    public AccountCreatedIntegrationEvent() { }
}
