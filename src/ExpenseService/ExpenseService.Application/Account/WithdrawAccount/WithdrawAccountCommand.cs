using ExpenseService.Application.Common;
using ExpenseService.Application.Models.Accounts;
using ExpenseService.Domain.Transaction;
using ExpenseTracker.Application.Common.Exceptions;
using ExpenseTracker.Contracts.Account;
using ExpenseTracker.Contracts.Enums;

namespace ExpenseService.Application.Account.WithdrawAccount;

public record WithdrawAccountCommand
{
    public WithdrawAccountCommand(Guid accountId, long expectedVersion, decimal amount)
    {
        AccountId = accountId;
        ExpectedVersion = expectedVersion;
        Amount = amount;
    }

    public Guid AccountId { get; set; }

    public long ExpectedVersion { get; set; }

    public decimal Amount { get; set; }
}

public class WithdrawAccountCommandHandler
{
    public static async Task<AccountCommandResult> Handle(
        WithdrawAccountCommand request,
        IUnitOfWork unitOfWork,
        CancellationToken cancellationToken)
    {
        var transactionId = Guid.NewGuid();
        var accountAggregate = await unitOfWork.Accounts.LoadAsync(request.AccountId, cancellationToken) ?? throw new NotFoundException("Account not found");
        accountAggregate.Withdraw(request.Amount, transactionId);
        await unitOfWork.Accounts.SaveAsync(accountAggregate, request.ExpectedVersion, cancellationToken);

        var transactionAggregate = new TransactionAggregate(transactionId, request.Amount, TransactionType.Withdraw, accountAggregate.Id);
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
