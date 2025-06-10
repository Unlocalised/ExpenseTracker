using ExpenseTracker.Application.Common.Exceptions;
using ExpenseTracker.Domain.Transaction;
using ExpenseTracker.Application.Common;
using ExpenseTracker.Domain.Enums;
using MediatR;

namespace ExpenseService.Application.Account.DepositAccount;

public record DepositAccountCommand : IRequest<Guid>
{
    public DepositAccountCommand(Guid accountId, long currentVersion, decimal amount)
    {
        AccountId = accountId;
        CurrentVersion = currentVersion;
        Amount = amount;
    }

    public Guid AccountId { get; set; }

    public long CurrentVersion { get; set; }

    public decimal Amount { get; set; }
}

public class DepositAccountCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<DepositAccountCommand, Guid>
{
    public async Task<Guid> Handle(
        DepositAccountCommand request,
        CancellationToken cancellationToken)
    {
        var transactionId = Guid.NewGuid();
        var accountAggregate = await unitOfWork.Accounts.LoadAsync(request.AccountId, cancellationToken) ?? throw new NotFoundException("Account not found");
        accountAggregate.Deposit(request.Amount, transactionId);
        await unitOfWork.Accounts.SaveAsync(accountAggregate, request.CurrentVersion, cancellationToken);

        var transactionAggregate = new TransactionAggregate(transactionId, request.Amount, TransactionType.Deposit, accountAggregate.Id);
        unitOfWork.Transactions.Create(transactionAggregate);

        await unitOfWork.CommitAsync(cancellationToken);
        return transactionId;
    }
}
