using ExpenseTracker.Domain.Transaction.Events;
using ExpenseTracker.Domain.Common;
using ExpenseTracker.Domain.Enums;
using ExpenseTracker.Domain.Account.Events;
using ExpenseTracker.Domain.Account;
using System.Xml.Linq;

namespace ExpenseTracker.Domain.Transaction;

public class TransactionAggregate : BaseAggregate
{
    public decimal Amount { get; set; }

    public TransactionType TransactionType { get; set; }

    public Guid AccountId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public TransactionAggregate()
    {

    }

    public TransactionAggregate(Guid id, decimal amount, TransactionType transactionType, Guid accountId)
    {
        var @event = TransactionCreatedEvent.Create(id, DateTime.UtcNow, amount, transactionType, accountId);
        Enqueue(@event);
        Apply(@event);
    }

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
        AccountId = @event.AccountId;
        CreatedAt = @event.CreatedAt;
    }
}
