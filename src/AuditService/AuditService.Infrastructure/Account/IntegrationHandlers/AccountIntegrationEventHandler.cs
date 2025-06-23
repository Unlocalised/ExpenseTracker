using ExpenseTracker.Contracts.Account;
using Marten;

namespace AuditService.Infrastructure.Account.IntegrationHandlers;
public class AccountIntegrationEventHandler
{
    public static async Task Handle(AccountCreatedIntegrationEvent message, IDocumentSession session)
    {
        session.Events.StartStream(message.Id, message);
        await session.SaveChangesAsync();
    }

    public static async Task Handle(AccountBalanceUpdatedIntegrationEvent message, IDocumentSession session)
    {
        await session.Events.AppendExclusive(message.Id, message);
        await session.SaveChangesAsync();
    }

    public static async Task Handle(AccountDeletedIntegrationEvent message, IDocumentSession session)
    {
        await session.Events.AppendExclusive(message.Id, message);
        await session.SaveChangesAsync();
    }

    public static async Task Handle(AccountUpdatedIntegrationEvent message, IDocumentSession session)
    {
        await session.Events.AppendExclusive(message.Id, message);
        await session.SaveChangesAsync();
    }
}
