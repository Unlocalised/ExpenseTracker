using ExpenseTracker.Domain.Account.Events;
using ExpenseTracker.Domain.Common;
using MediatR;

namespace ExpenseTracker.Domain.Account;

public class AccountAggregate : BaseAggregate
{
    public string Name { get; private set; } = string.Empty;

    public decimal Balance { get; private set; }

    public string? Number { get; private set; }

    public string? BankName { get; private set; }

    public string? BankPhone { get; private set; }

    public string? BankAddress { get; private set; }

    public bool Enabled { get; private set; }

    public DateTime? CreatedAt { get; private set; }

    public DateTime? UpdatedAt { get; private set; }

    public DateTime? DeletedAt { get; private set; }

    public AccountAggregate() { }

    public AccountAggregate(Guid id, DateTime createdAt, string name, string? number, string? bankName, string? bankPhone, string? bankAddress)
    {
        var @event = AccountCreatedEvent.Create(id, createdAt, name, number, bankName, bankPhone, bankAddress);
        Enqueue(@event);
        Apply(@event);
    }

    public static AccountAggregate Create(AccountCreatedEvent @event)
    {
        return new AccountAggregate()
        {
            Id = @event.Id,
            BankAddress = @event.BankAddress,
            BankName = @event.BankName,
            BankPhone = @event.BankPhone,
            CreatedAt = @event.CreatedAt,
            Enabled = true,
            Name = @event.Name,
            Number = @event.Number
        };
    }

    public void Withdraw(decimal amount, Guid transactionId)
    {
        var @event = AccountWithdrawalEvent.Create(Id, transactionId, amount);
        Enqueue(@event);
        Apply(@event);
    }

    public void Deposit(decimal amount, Guid transactionId)
    {
        var @event = AccountDepositEvent.Create(Id, transactionId, amount);
        Enqueue(@event);
        Apply(@event);
    }

    public void Update(string? name = null, string? number = null,
        string? bankName = null, string? bankPhone = null, string? bankAddress = null, bool? enabled = null)
    {
        if (DeletedAt.HasValue)
            throw new DomainException("Cannot update a deleted account.");
        var @event = AccountUpdatedEvent.Create(Id, DateTime.UtcNow, name, number, bankName, bankPhone, bankAddress, enabled);
        Enqueue(@event);
        Apply(@event);
    }

    public void Delete()
    {
        if (DeletedAt.HasValue)
            return;
        var @event = AccountDeletedEvent.Create(Id, DateTime.UtcNow);
        Enqueue(@event);
        Apply(@event);
    }

    public void Apply(AccountCreatedEvent @event)
    {
        Id = @event.Id;
        Name = @event.Name;
        CreatedAt = @event.CreatedAt;
        Number = @event.Number;
        BankName = @event.BankName;
        BankPhone = @event.BankPhone;
        BankAddress = @event.BankAddress;
        Enabled = true;
    }

    public void Apply(AccountUpdatedEvent @event)
    {
        if (!string.IsNullOrEmpty(@event.Name))
            Name = @event.Name;
        if (@event.Number is not null)
            Number = @event.Number;
        if (@event.BankName is not null)
            BankName = @event.BankName;
        if (@event.BankPhone is not null)
            BankPhone = @event.BankPhone;
        if (@event.BankAddress is not null)
            BankAddress = @event.BankAddress;
        if (@event.Enabled.HasValue)
            Enabled = @event.Enabled.Value;
        UpdatedAt = @event.UpdatedAt;
    }

    public void Apply(AccountDeletedEvent @event)
    {
        Enabled = false;
        DeletedAt = @event.DeletedAt;
    }

    public void Apply(AccountWithdrawalEvent @event)
    {
        Balance -= @event.Amount;
    }

    public void Apply(AccountDepositEvent @event)
    {
        Balance += @event.Amount;
    }
}
