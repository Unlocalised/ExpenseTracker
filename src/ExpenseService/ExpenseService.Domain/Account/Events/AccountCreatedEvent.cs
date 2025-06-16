using ExpenseTracker.BuildingBlocks.Common;

namespace ExpenseService.Domain.Account.Events;
public record AccountCreatedEvent : BaseEvent
{
    public Guid Id { get; init; }

    public DateTime CreatedAt { get; set; }

    public required string Name { get; set; }

    public string? Number { get; set; }

    public string? BankName { get; set; }

    public string? BankPhone { get; set; }

    public string? BankAddress { get; set; }

    public AccountCreatedEvent() { }

    public static AccountCreatedEvent Create(
        Guid id,
        DateTime createdAt,
        string name,
        string? number,
        string? bankName,
        string? bankPhone,
        string? bankAddress) => new()
        {
            Id = id,
            CreatedAt = createdAt,
            Name = name,
            Number = number,
            BankName = bankName,
            BankPhone = bankPhone,
            BankAddress = bankAddress
        };
}
