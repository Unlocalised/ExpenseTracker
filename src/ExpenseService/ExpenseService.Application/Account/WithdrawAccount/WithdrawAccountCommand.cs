using ExpenseTracker.Application.Account;
using ExpenseTracker.Application.Common.Exceptions;
using ExpenseTracker.Application.Transaction;
using ExpenseTracker.Domain.Enums;
using ExpenseTracker.Domain.Transaction;
using ExpenseTracker.Domain.Transaction.Events;
using MediatR;

namespace ExpenseService.Application.Account.WithdrawAccount;

public record WithdrawAccountCommand : IRequest<Guid>
{
    public Guid AccountId { get; set; }

    public long Version { get; set; }

    public decimal Amount { get; set; }
}

public class WithdrawAccountCommandHandler(IAccountRepository accountRepository, ITransactionRepository transactionRepository) : IRequestHandler<WithdrawAccountCommand, Guid>
{
    public async Task<Guid> Handle(
        WithdrawAccountCommand request,
        CancellationToken cancellationToken)
    {
        var transactionId = Guid.NewGuid();
        var accountAggregate = await accountRepository.AggregateStreamAsync(request.AccountId, cancellationToken) ?? throw new NotFoundException("Account not found");
        accountAggregate.Withdraw(request.Amount, transactionId);
        accountRepository.Persist(accountAggregate.Id, request.Version + 1, accountAggregate.DequeueUncommittedEvents());

        var transactionAggregate = TransactionAggregate.Create(TransactionCreatedEvent.Create(transactionId, DateTime.Now, request.Amount, TransactionType.Withdraw, accountAggregate.Id));
        transactionRepository.StartStream(transactionAggregate);

        await accountRepository.SaveChangesAsync(cancellationToken);
        return transactionId;
    }
}
