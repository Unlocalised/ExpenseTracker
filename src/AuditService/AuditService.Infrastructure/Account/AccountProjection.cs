using AuditService.Domain.Account;
using ExpenseTracker.Contracts.Account;
using JasperFx.Events;
using Marten.Events.Aggregation;

namespace AuditService.Infrastructure.Account;

public class AccountProjection : SingleStreamProjection<AccountReadModel, Guid>
{
    public AccountProjection()
    {
        ProjectEvent<IEvent<AccountCreatedIntegrationEvent>>((view, @event) =>
        {
            var data = @event.Data;
            view.Id = data.Id;
            view.Name = data.Name;
            view.Number = data.Number;
            view.BankName = data.BankName;
            view.BankPhone = data.BankPhone;
            view.BankAddress = data.BankAddress;
            view.Enabled = true;
            view.ExpectedVersion = data.ExpectedVersion;
            view.CreatedAt = data.CreatedAt;
        });
        ProjectEvent<IEvent<AccountBalanceUpdatedIntegrationEvent>>((view, @event) =>
        {
            var data = @event.Data;
            view.Balance = data.Balance;
            view.UpdatedAt = data.UpdatedAt;
            view.ExpectedVersion = data.ExpectedVersion;
        });
        ProjectEvent<IEvent<AccountUpdatedIntegrationEvent>>((view, @event) =>
        {
            var data = @event.Data;
            if (!string.IsNullOrEmpty(data.Name))
                view.Name = data.Name;
            view.Number = data.Number;
            view.BankName = data.BankName;
            view.BankPhone = data.BankPhone;
            view.BankAddress = data.BankAddress;
            view.UpdatedAt = data.UpdatedAt;
            if (data.Enabled.HasValue)
                view.Enabled = data.Enabled.Value;
            view.ExpectedVersion = data.ExpectedVersion;
        });
        ProjectEvent<IEvent<AccountDeletedIntegrationEvent>>((view, @event) =>
        {
            var data = @event.Data;
            view.DeletedAt = data.DeletedAt;
            view.Enabled = false;
        });
    }

}
