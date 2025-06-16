using ExpenseTracker.Contracts.Enums;

namespace ExpenseTracker.Contracts.Transaction;
public record TransactionCreatedIntegrationEvent
{
    public Guid Id { get; init; }

    public DateTime CreatedAt { get; set; }

    public decimal Amount { get; set; }

    public TransactionType TransactionType { get; set; }

    public Guid AccountId { get; set; }

    public TransactionCreatedIntegrationEvent() { }

    public static TransactionCreatedIntegrationEvent Create(
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
