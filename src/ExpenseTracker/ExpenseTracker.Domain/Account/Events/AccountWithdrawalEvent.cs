using ExpenseTracker.Domain.Common;

namespace ExpenseTracker.Domain.Account.Events;
public record AccountWithdrawalEvent : BaseEvent
{
    public Guid AccountId { get; set; }

    public Guid TransactionId { get; set; }

    public decimal Amount { get; set; }

    public AccountWithdrawalEvent() { }

    public static AccountWithdrawalEvent Create(
        Guid accountId,
        Guid transactionId,
        decimal amount) => new()
        {
            AccountId = accountId,
            TransactionId = transactionId,
            Amount = amount
        };
}