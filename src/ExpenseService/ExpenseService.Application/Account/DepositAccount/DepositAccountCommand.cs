using ExpenseTracker.Application.Common.Exceptions;
using ExpenseTracker.Application.Transaction;
using ExpenseTracker.Application.Account;
using ExpenseTracker.Domain.Transaction;
using ExpenseTracker.Domain.Enums;
using MediatR;
using ExpenseTracker.Domain.Transaction.Events;

namespace ExpenseService.Application.Account.DepositAccount;

public record DepositAccountCommand : IRequest<Guid>
{
    public Guid AccountId { get; set; }

    public long Version { get; set; }

    public decimal Amount { get; set; }
}

public class DepositAccountCommandHandler(IAccountRepository accountRepository, ITransactionRepository transactionRepository) : IRequestHandler<DepositAccountCommand, Guid>
{
    public async Task<Guid> Handle(
        DepositAccountCommand request,
        CancellationToken cancellationToken)
    {
        var transactionId = Guid.NewGuid();
        var accountAggregate = await accountRepository.AggregateStreamAsync(request.AccountId, cancellationToken) ?? throw new NotFoundException("Account not found");
        accountAggregate.Deposit(request.Amount, transactionId);
        accountRepository.Persist(accountAggregate.Id, request.Version + 1, accountAggregate.DequeueUncommittedEvents());

        var transactionAggregate = TransactionAggregate.Create(TransactionCreatedEvent.Create(transactionId, DateTime.Now, request.Amount, TransactionType.Deposit, accountAggregate.Id));
        transactionRepository.StartStream(transactionAggregate);

        await accountRepository.SaveChangesAsync(cancellationToken);
        return transactionId;
    }
}
