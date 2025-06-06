using ExpenseTracker.Domain.Transaction;
using ExpenseTracker.Domain.Transaction.Events;
using Marten.Events;
using Marten.Events.Projections;

namespace ExpenseService.Infrastructure.Transaction;

public class TransactionProjection : MultiStreamProjection<TransactionReadModel, Guid>
{
    public TransactionProjection()
    {
        Identity<TransactionCreatedEvent>(@event => @event.Id);
        ProjectEvent<IEvent<TransactionCreatedEvent>>((view, @event) =>
        {
            var data = @event.Data;
            view.Id = data.Id;
            view.AccountId = data.AccountId;
            view.Number = data.Number;
            view.CreatedAt = data.CreatedAt;
            view.TransactionType = data.TransactionType;
            view.Amount = data.Amount;
        });
    }

}
