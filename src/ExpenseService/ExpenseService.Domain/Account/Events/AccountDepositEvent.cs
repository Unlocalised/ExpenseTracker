using ExpenseTracker.BuildingBlocks.Common;

namespace ExpenseService.Domain.Account.Events;
public record AccountDepositEvent : BaseEvent
{
    public Guid AccountId { get; set; }

    public Guid TransactionId { get; set; }

    public decimal Amount { get; set; }

    public DateTime UpdatedAt { get; set; }

    public AccountDepositEvent() { }

    public static AccountDepositEvent Create(
        Guid accountId,
        Guid transactionId,
        decimal amount,
        DateTime updatedAt) => new()
        {
            AccountId = accountId,
            TransactionId = transactionId,
            Amount = amount,
            UpdatedAt = updatedAt
        };
}