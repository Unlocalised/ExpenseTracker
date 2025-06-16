using ExpenseService.Application.Common;
using ExpenseService.Application.Models.Accounts;
using ExpenseService.Domain.Transaction;
using ExpenseTracker.Application.Common.Exceptions;
using ExpenseTracker.Contracts.Account;
using ExpenseTracker.Contracts.Enums;

namespace ExpenseService.Application.Account.DepositAccount;

public record DepositAccountCommand
{
    public DepositAccountCommand(Guid accountId, long expectedVersion, decimal amount)
    {
        AccountId = accountId;
        ExpectedVersion = expectedVersion;
        Amount = amount;
    }

    public Guid AccountId { get; set; }

    public long ExpectedVersion { get; set; }

    public decimal Amount { get; set; }
}

public class DepositAccountCommandHandler
{
    public static async Task<AccountCommandResult> Handle(
        DepositAccountCommand request,
        IUnitOfWork unitOfWork,
        CancellationToken cancellationToken)
    {
        var transactionId = Guid.NewGuid();
        var accountAggregate = await unitOfWork.Accounts.LoadAsync(request.AccountId, cancellationToken) ?? throw new NotFoundException("Account not found");
        accountAggregate.Deposit(request.Amount, transactionId);
        await unitOfWork.Accounts.SaveAsync(accountAggregate, request.ExpectedVersion, cancellationToken);

        var transactionAggregate = new TransactionAggregate(transactionId, request.Amount, TransactionType.Deposit, accountAggregate.Id);
        unitOfWork.Transactions.Create(transactionAggregate);
        if (accountAggregate.UpdatedAt.HasValue)
            await unitOfWork.PublishAsync(new AccountBalanceUpdatedIntegrationEvent
            {
                Id = accountAggregate.Id,
                Balance = accountAggregate.Balance,
                UpdatedAt = accountAggregate.UpdatedAt.Value,
                ExpectedVersion = accountAggregate.Version
            });
        await unitOfWork.CommitAsync(cancellationToken);

        return new AccountCommandResult
        {
            AccountId = accountAggregate.Id,
            NewVersion = accountAggregate.Version
        };
    }
}
