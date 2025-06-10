using ExpenseTracker.Application.Common.Exceptions;
using ExpenseTracker.Application.Common;
using ExpenseTracker.Domain.Transaction;
using ExpenseTracker.Domain.Enums;
using MediatR;
using ExpenseService.Application.Models.Accounts;

namespace ExpenseService.Application.Account.WithdrawAccount;

public record WithdrawAccountCommand : IRequest<AccountCommandResult>
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

public class WithdrawAccountCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<WithdrawAccountCommand, AccountCommandResult>
{
    public async Task<AccountCommandResult> Handle(
        WithdrawAccountCommand request,
        CancellationToken cancellationToken)
    {
        var transactionId = Guid.NewGuid();
        var accountAggregate = await unitOfWork.Accounts.LoadAsync(request.AccountId, cancellationToken) ?? throw new NotFoundException("Account not found");
        accountAggregate.Withdraw(request.Amount, transactionId);
        await unitOfWork.Accounts.SaveAsync(accountAggregate, request.ExpectedVersion, cancellationToken);

        var transactionAggregate = new TransactionAggregate(transactionId, request.Amount, TransactionType.Withdraw, accountAggregate.Id, DateTime.UtcNow);
        unitOfWork.Transactions.Create(transactionAggregate);

        await unitOfWork.CommitAsync(cancellationToken);
        
        return new AccountCommandResult
        {
            AccountId = accountAggregate.Id,
            NewVersion = accountAggregate.Version
        };
    }
}
