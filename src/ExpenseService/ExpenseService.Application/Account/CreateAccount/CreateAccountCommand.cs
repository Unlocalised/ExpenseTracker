using ExpenseTracker.Application.Common;
using ExpenseTracker.Domain.Account;
using ExpenseTracker.Domain.Enums;
using ExpenseTracker.Domain.Transaction;
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

public class CreateAccountCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<CreateAccountCommand, Guid>
{
    public async Task<Guid> Handle(
        CreateAccountCommand request,
        CancellationToken cancellationToken)
    {
        var accountAggregate = new AccountAggregate(Guid.NewGuid(),
                DateTime.UtcNow,
                request.Name,
                request.Number,
                request.BankName,
                request.BankPhone,
                request.BankAddress);

        if (request.OpeningBalance.HasValue && request.OpeningBalance.Value > 0)
        {
            var transactionAggregate = new TransactionAggregate(Guid.NewGuid(), request.OpeningBalance.Value, TransactionType.Deposit, accountAggregate.Id);
            unitOfWork.Transactions.Create(transactionAggregate);
            accountAggregate.Deposit(request.OpeningBalance.Value, transactionAggregate.Id);
        }
        unitOfWork.Accounts.Create(accountAggregate);
        await unitOfWork.CommitAsync(cancellationToken);
        return accountAggregate.Id;
    }
}
