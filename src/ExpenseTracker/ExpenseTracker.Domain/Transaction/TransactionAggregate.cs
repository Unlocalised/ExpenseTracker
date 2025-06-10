using ExpenseTracker.Domain.Transaction.Events;
using ExpenseTracker.Domain.Common;
using ExpenseTracker.Domain.Enums;
using ExpenseTracker.Domain.Account.Events;
using static System.Runtime.InteropServices.JavaScript.JSType;
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

    public TransactionAggregate(Guid id, decimal amount, TransactionType transactionType, Guid accountId, DateTime createdAt)
    {
        var @event = TransactionCreatedEvent.Create(id, createdAt, amount, transactionType, accountId);
        Enqueue(@event);
        Apply(@event);
    }

    public void Apply(TransactionCreatedEvent @event)
    {
        Id = @event.Id;
        Amount = @event.Amount;
        TransactionType = @event.TransactionType;
        AccountId = @event.AccountId;
        CreatedAt = @event.CreatedAt;

        Version++;
    }
}
