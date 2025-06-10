using ExpenseTracker.Domain.Common;
using ExpenseTracker.Domain.Enums;

namespace ExpenseTracker.Domain.Transaction.Events;
public record TransactionCreatedEvent : BaseEvent
{
    public Guid Id { get; init; }

    public DateTime CreatedAt { get; set; }

    public decimal Amount { get; set; }

    public TransactionType TransactionType { get; set; }

    public Guid AccountId { get; set; }

    public TransactionCreatedEvent() { }

    public static TransactionCreatedEvent Create(
        Guid id,
        DateTime createdAt,
        decimal amount,
        TransactionType transactionType,
        Guid accountId) => new()
        {
            Id = id,
            CreatedAt = createdAt,
            Amount = amount,
            TransactionType = transactionType,
            AccountId = accountId
        };
}
