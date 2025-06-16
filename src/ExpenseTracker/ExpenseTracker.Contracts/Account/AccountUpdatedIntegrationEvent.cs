namespace ExpenseTracker.Contracts.Account;
public record AccountUpdatedIntegrationEvent
{
    public Guid Id { get; set; }

    public DateTime UpdatedAt { get; set; }

    public long ExpectedVersion { get; set; }

    public string? Name { get; set; }

    public string? Number { get; set; }

    public string? BankName { get; set; }

    public string? BankPhone { get; set; }

    public string? BankAddress { get; set; }

    public bool? Enabled { get; set; }

    public AccountUpdatedIntegrationEvent() { }
}
