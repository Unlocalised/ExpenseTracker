using ExpenseTracker.Domain.Transaction;
using ExpenseTracker.Domain.Transaction.Events;
using Marten.Events.Aggregation;
using JasperFx.Events;

namespace ExpenseService.Infrastructure.Transaction;

public class TransactionProjection : SingleStreamProjection<TransactionReadModel, Guid>
{
    public TransactionProjection()
    {
        ProjectEvent<IEvent<TransactionCreatedEvent>>((view, @event) =>
        {
            var data = @event.Data;
            view.Id = data.Id;
            view.AccountId = data.AccountId;
            view.CreatedAt = data.CreatedAt;
            view.TransactionType = data.TransactionType;
            view.Amount = data.Amount;
        });
    }

}
