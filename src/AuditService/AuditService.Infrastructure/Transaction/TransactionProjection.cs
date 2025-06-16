using AuditService.Domain.Transaction;
using ExpenseTracker.Contracts.Transaction;
using JasperFx.Events;
using Marten.Events.Aggregation;

namespace AuditService.Infrastructure.Transaction;

public class TransactionProjection : SingleStreamProjection<TransactionReadModel, Guid>
{
    public TransactionProjection()
    {
        ProjectEvent<IEvent<TransactionCreatedIntegrationEvent>>((view, @event) =>
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
