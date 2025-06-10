using ExpenseTracker.Application.Common.Exceptions;
using ExpenseTracker.Application.Common;
using ExpenseTracker.Domain.Transaction;
using ExpenseTracker.Domain.Enums;
using MediatR;

namespace ExpenseService.Application.Account.WithdrawAccount;

public record WithdrawAccountCommand : IRequest<Guid>
{
    public WithdrawAccountCommand(Guid accountId, long currentVersion, decimal amount)
    {
        AccountId = accountId;
        CurrentVersion = currentVersion;
        Amount = amount;
    }

    public Guid AccountId { get; set; }

    public long CurrentVersion { get; set; }

    public decimal Amount { get; set; }
}

public class WithdrawAccountCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<WithdrawAccountCommand, Guid>
{
    public async Task<Guid> Handle(
        WithdrawAccountCommand request,
        CancellationToken cancellationToken)
    {
        var transactionId = Guid.NewGuid();
        var accountAggregate = await unitOfWork.Accounts.LoadAsync(request.AccountId, cancellationToken) ?? throw new NotFoundException("Account not found");
        accountAggregate.Withdraw(request.Amount, transactionId);
        await unitOfWork.Accounts.SaveAsync(accountAggregate, request.CurrentVersion, cancellationToken);

        var transactionAggregate = new TransactionAggregate(transactionId, request.Amount, TransactionType.Withdraw, accountAggregate.Id);
        unitOfWork.Transactions.Create(transactionAggregate);

        await unitOfWork.CommitAsync(cancellationToken);
        return transactionId;
    }
}
