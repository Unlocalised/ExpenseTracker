using ExpenseService.Application.Models.Accounts;
using ExpenseService.Domain.Transaction;
using ExpenseService.Application.Common;
using ExpenseTracker.Contracts.Account;
using ExpenseTracker.Contracts.Enums;
using ExpenseService.Domain.Account;

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
    public static async Task<AccountCommandResult> Handle(
        CreateAccountCommand request,
        IUnitOfWork unitOfWork,
        CancellationToken cancellationToken)
    {
        var accountAggregate = new AccountAggregate(
            Guid.NewGuid(),
            request.Name,
            request.Number,
            request.BankName,
            request.BankPhone,
            request.BankAddress);

        if (request.OpeningBalance.HasValue && request.OpeningBalance.Value > 0)
        {
            var transactionAggregate = new TransactionAggregate(Guid.NewGuid(),
                request.OpeningBalance.Value,
                TransactionType.Deposit,
                accountAggregate.Id);
            unitOfWork.Transactions.Create(transactionAggregate);
            accountAggregate.Deposit(request.OpeningBalance.Value, transactionAggregate.Id);
        }
        unitOfWork.Accounts.Create(accountAggregate);
        await unitOfWork.PublishAsync(new AccountCreatedIntegrationEvent
        {
            Id = accountAggregate.Id,
            Balance = accountAggregate.Balance,
            BankName = accountAggregate.BankName,
            BankPhone = accountAggregate.BankPhone,
            BankAddress = accountAggregate.BankAddress,
            CreatedAt = accountAggregate.CreatedAt,
            ExpectedVersion = accountAggregate.Version,
            Name = accountAggregate.Name,
            Number = accountAggregate.Number
        });
        await unitOfWork.CommitAsync(cancellationToken);

        return new AccountCommandResult
        {
            AccountId = accountAggregate.Id,
            NewVersion = accountAggregate.Version
        };
    }
}
