using ExpenseService.Domain.Transaction;
using ExpenseService.Domain.Transaction.Events;

namespace ExpenseService.UnitTests.Domain;

public class TransactionAggregateTests
{
    [Fact]
    public void Create_ShouldCreateAndApplyAndEnqueueEvent()
    {
        var transactionAggregate = new TransactionAggregate(Guid.NewGuid(), 0, ExpenseTracker.Contracts.Enums.TransactionType.Withdraw, Guid.NewGuid());

        Assert.Equal(1, transactionAggregate.Version);
        var events = transactionAggregate.DequeueUncommittedEvents();
        Assert.NotEmpty(events);
        Assert.Single(events);
        var @event = Assert.IsType<TransactionCreatedEvent>(events[0]);
        Assert.Equal(transactionAggregate.Id, @event.Id);
    }
}