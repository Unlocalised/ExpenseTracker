using ExpenseTracker.Application.Common.Exceptions;
using ExpenseService.Application.Models.Accounts;
using ExpenseTracker.Domain.Transaction;
using ExpenseTracker.Application.Common;
using ExpenseTracker.Domain.Enums;

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

        var transactionAggregate = new TransactionAggregate(transactionId, request.Amount, TransactionType.Deposit, accountAggregate.Id, DateTime.UtcNow);
        unitOfWork.Transactions.Create(transactionAggregate);

        await unitOfWork.CommitAsync(cancellationToken);

        return new AccountCommandResult
        {
            AccountId = accountAggregate.Id,
            NewVersion = accountAggregate.Version
        };
    }
}
