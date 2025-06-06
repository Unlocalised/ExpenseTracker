using ExpenseTracker.Domain.Transaction.Events;
using ExpenseTracker.Domain.Common;
using ExpenseTracker.Domain.Enums;
using ExpenseTracker.Domain.Account.Events;
using ExpenseTracker.Domain.Account;

namespace ExpenseTracker.Domain.Transaction;

public class TransactionAggregate : BaseAggregate
{
    public decimal Amount { get; set; }

    public TransactionType TransactionType { get; set; }

    public Guid AccountId { get; set; }

    public string? Number { get; set; }

    public DateTime? CreatedAt { get; set; }

    public static TransactionAggregate Create(TransactionCreatedEvent @event)
    {
        var account = new TransactionAggregate();
        account.Enqueue(@event);
        account.Apply(@event);

        return account;
    }

    public void Apply(TransactionCreatedEvent @event)
    {
        Id = @event.Id;
        Amount = @event.Amount;
        TransactionType = @event.TransactionType;
        Number = @event.Number;
        AccountId = @event.AccountId;
        CreatedAt = @event.CreatedAt;
    }
}
