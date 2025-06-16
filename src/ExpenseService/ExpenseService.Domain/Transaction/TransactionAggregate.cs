using ExpenseService.Domain.Transaction.Events;
using ExpenseTracker.BuildingBlocks.Common;
using ExpenseTracker.Contracts.Enums;

namespace ExpenseService.Domain.Transaction;

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
