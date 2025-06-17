using ExpenseService.Domain.Account;
using ExpenseService.Domain.Account.Events;
using ExpenseTracker.BuildingBlocks.Common;

namespace ExpenseService.UnitTests;

public class AccountUnitTest
{
    [Fact]
    public void Create_ShouldCreateAndApplyAndEnqueueEvent()
    {
        var accountAggregate = new AccountAggregate(Guid.NewGuid(), string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);

        Assert.Equal(2, accountAggregate.Version);
        var events = accountAggregate.DequeueUncommittedEvents();
        Assert.NotEmpty(events);
        Assert.Single(events);
        var @event = Assert.IsType<AccountCreatedEvent>(events[0]);
        Assert.Equal(accountAggregate.Id, @event.Id);
    }

    [Fact]
    public void Deposit_ShouldCreateAndApplyAndEnqueueEvent()
    {
        var accountAggregate = new AccountAggregate();
        var transactionId = Guid.NewGuid();
        var amount = 100;
        accountAggregate.Deposit(amount, transactionId);

        Assert.Equal(2, accountAggregate.Version);
        Assert.Equal(accountAggregate.Balance, amount);
        var events = accountAggregate.DequeueUncommittedEvents();
        Assert.NotEmpty(events);
        Assert.Single(events);
        var @event = Assert.IsType<AccountDepositEvent>(events[0]);
        Assert.Equal(transactionId, @event.TransactionId);
        Assert.Equal(amount, @event.Amount);
    }

    [Fact]
    public void Withdraw_ShouldCreateAndApplyAndEnqueueTheEvent()
    {
        var accountAggregate = new AccountAggregate();
        var transactionId = Guid.NewGuid();
        var amount = 100;
        accountAggregate.Deposit(200, transactionId);
        accountAggregate.DequeueUncommittedEvents();
        accountAggregate.Withdraw(amount, transactionId);

        Assert.Equal(3, accountAggregate.Version);
        Assert.Equal(amount, accountAggregate.Balance);
        var events = accountAggregate.DequeueUncommittedEvents();
        Assert.NotEmpty(events);
        Assert.Single(events);
        var @event = Assert.IsType<AccountWithdrawalEvent>(events[0]);
        Assert.Equal(transactionId, @event.TransactionId);
        Assert.Equal(amount, @event.Amount);
    }

    [Fact]
    public void Withdraw_ShouldThrow_WhenInsufficientBalance()
    {
        var accountAggregate = new AccountAggregate();
        var transactionId = Guid.NewGuid();
        Assert.Throws<DomainException>(() => accountAggregate.Withdraw(100, transactionId));
    }

    [Fact]
    public void Delete_ShouldCreateAndApplyAndEnqueueTheEvent()
    {
        var accountAggregate = new AccountAggregate();
        accountAggregate.Delete();

        Assert.Equal(2, accountAggregate.Version);
        Assert.True(accountAggregate.DeletedAt.HasValue);
        Assert.False(accountAggregate.Enabled);
        var events = accountAggregate.DequeueUncommittedEvents();
        Assert.NotEmpty(events);
        Assert.Single(events);
        var @event = Assert.IsType<AccountDeletedEvent>(events[0]);
        Assert.Equal(accountAggregate.DeletedAt.Value, @event.DeletedAt);
    }

    [Fact]
    public void Update_ShouldCreateAndApplyAndEnqueueTheEvent()
    {
        var accountAggregate = new AccountAggregate();
        accountAggregate.Update();

        Assert.Equal(2, accountAggregate.Version);
        Assert.True(accountAggregate.UpdatedAt.HasValue);
        Assert.False(accountAggregate.Enabled);
        var events = accountAggregate.DequeueUncommittedEvents();
        Assert.NotEmpty(events);
        Assert.Single(events);
        var @event = Assert.IsType<AccountUpdatedEvent>(events[0]);
        Assert.Equal(accountAggregate.UpdatedAt.Value, @event.UpdatedAt);
    }

    [Fact]
    public void Update_ShouldThrow_WhenAccountDeleted()
    {
        var accountAggregate = new AccountAggregate();
        accountAggregate.Delete();

        Assert.Throws<DomainException>(() => accountAggregate.Update());
    }
}