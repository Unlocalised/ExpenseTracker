using AuditService.Application.Account.GetAccounts;
using AuditService.Domain.Account;
using AuditService.IntegrationTests.Setup;
using ExpenseTracker.Contracts.Account;

namespace AuditService.IntegrationTests.Handlers;

public class GetAccountsTests : IntegrationTest
{
    [Fact]
    public async Task GetAccounts_ShouldReturnAccountsListWhenExists()
    {
        var accountCreatedEvent = new AccountCreatedIntegrationEvent
        {
            Id = Guid.NewGuid(),
            CreatedAt = DateTime.UtcNow,
            ExpectedVersion = 1,
        };
        DocumentSession.Events.StartStream(accountCreatedEvent.Id, accountCreatedEvent);
        await DocumentSession.SaveChangesAsync();
        await GenerateProjectionsAsync();

        var accounts = await MessageBus.InvokeAsync<IReadOnlyList<AccountReadModel>>(new GetAccountsQuery());

        Assert.NotNull(accounts);
        Assert.IsType<IReadOnlyList<AccountReadModel>>(accounts, exactMatch: false);
        Assert.NotEmpty(accounts);
        var account = Assert.Single(accounts);
        Assert.NotNull(account);
        Assert.IsType<AccountReadModel>(account);
        Assert.Equal(accountCreatedEvent.Id, account.Id);
        Assert.Equal(accountCreatedEvent.ExpectedVersion, account.ExpectedVersion);
        Assert.Equal(accountCreatedEvent.Balance, account.Balance);
        Assert.Equal(accountCreatedEvent.CreatedAt, account.CreatedAt);
    }

    [Fact]
    public async Task GetAccounts_ShouldReturnEmptyAccountsListWhenNoneExists()
    {
        var accounts = await MessageBus.InvokeAsync<IReadOnlyList<AccountReadModel>>(new GetAccountsQuery());

        Assert.NotNull(accounts);
        Assert.IsType<IReadOnlyList<AccountReadModel>>(accounts, exactMatch: false);
        Assert.Empty(accounts);
    }
}