using ExpenseTracker.Domain.Common;

namespace ExpenseTracker.Domain.Account.Events;
public record AccountUpdatedEvent : BaseEvent
{
    public Guid Id { get; set; }

    public DateTime UpdatedAt { get; set; }

    public string? Name { get; set; }

    public string? Number { get; set; }

    public string? BankName { get; set; }

    public string? BankPhone { get; set; }

    public string? BankAddress { get; set; }

    public bool? Enabled { get; set; }

    public AccountUpdatedEvent() { }

    public static AccountUpdatedEvent Create(
        Guid id,
        DateTime updatedAt,
        string? name,
        string? number,
        string? bankName,
        string? bankPhone,
        string? bankAddress,
        bool? enabled) => new()
        {
            Id = id,
            UpdatedAt = updatedAt,
            Name = name,
            Number = number,
            BankName = bankName,
            BankPhone = bankPhone,
            BankAddress = bankAddress,
            Enabled = enabled
        };
}