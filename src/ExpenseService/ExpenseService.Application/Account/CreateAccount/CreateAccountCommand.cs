using ExpenseTracker.Domain.Transaction;
using ExpenseTracker.Application.Common;
using ExpenseTracker.Domain.Account;
using ExpenseTracker.Domain.Enums;

namespace ExpenseService.Application.Account.CreateAccount;

public record CreateAccountCommand
{
    public required string Name { get; set; }

    public decimal? OpeningBalance { get; set; }

    public string? Number { get; set; }

    public string? BankName { get; set; }

    public string? BankPhone { get; set; }

    public string? BankAddress { get; set; }
}

public class CreateAccountCommandHandler
{
    public static async Task<Guid> Handle(
        CreateAccountCommand request,
        IUnitOfWork unitOfWork,
        CancellationToken cancellationToken)
    {
        var accountAggregate = new AccountAggregate(Guid.NewGuid(),
                request.Name,
                request.Number,
                request.BankName,
                request.BankPhone,
                request.BankAddress,
                DateTime.UtcNow);

        if (request.OpeningBalance.HasValue && request.OpeningBalance.Value > 0)
        {
            var transactionAggregate = new TransactionAggregate(Guid.NewGuid(), request.OpeningBalance.Value, TransactionType.Deposit, accountAggregate.Id, DateTime.UtcNow);
            unitOfWork.Transactions.Create(transactionAggregate);
            accountAggregate.Deposit(request.OpeningBalance.Value, transactionAggregate.Id);
        }
        unitOfWork.Accounts.Create(accountAggregate);
        await unitOfWork.CommitAsync(cancellationToken);

        return accountAggregate.Id;
    }
}
