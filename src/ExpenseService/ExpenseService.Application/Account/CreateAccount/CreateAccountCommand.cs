using ExpenseTracker.Application.Account;
using ExpenseTracker.Application.Transaction;
using ExpenseTracker.Domain.Account;
using ExpenseTracker.Domain.Account.Events;
using ExpenseTracker.Domain.Enums;
using ExpenseTracker.Domain.Transaction;
using ExpenseTracker.Domain.Transaction.Events;
using MediatR;

namespace ExpenseService.Application.Account.CreateAccount;

public record CreateAccountCommand : IRequest<Guid>
{
    public required string Name { get; set; }

    public decimal? OpeningBalance { get; set; }

    public string? Number { get; set; }

    public string? BankName { get; set; }

    public string? BankPhone { get; set; }

    public string? BankAddress { get; set; }
}

public class CreateAccountCommandHandler(IAccountRepository accountRepository, ITransactionRepository transactionRepository) : IRequestHandler<CreateAccountCommand, Guid>
{
    public async Task<Guid> Handle(
        CreateAccountCommand request,
        CancellationToken cancellationToken)
    {
        var accountAggregate = new AccountAggregate(Guid.NewGuid(),
                DateTime.Now,
                request.Name,
                request.Number,
                request.BankName,
                request.BankPhone,
                request.BankAddress);

        if (request.OpeningBalance.HasValue)
        {
            TransactionAggregate transactionAggregate = TransactionAggregate.Create(
                TransactionCreatedEvent.Create(Guid.NewGuid(), DateTime.Now, request.OpeningBalance.Value, TransactionType.Deposit, accountAggregate.Id));
            accountAggregate.Deposit(request.OpeningBalance.Value, transactionAggregate.Id);
            transactionRepository.StartStream(transactionAggregate);
        }

        accountRepository.StartStream(accountAggregate);
        await accountRepository.SaveChangesAsync(cancellationToken);
        return accountAggregate.Id;
    }
}
