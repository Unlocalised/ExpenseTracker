using ExpenseTracker.Application.Common.Exceptions;
using ExpenseService.Application.Models.Accounts;
using ExpenseTracker.Application.Common;

namespace ExpenseService.Application.Account.UpdateAccount;

public record UpdateAccountCommand
{
    public UpdateAccountCommand(Guid id, long expectedVersion, string? name, string? number, string? bankName, string? bankPhone, string? bankAddress, bool? enabled)
    {
        Id = id;
        ExpectedVersion = expectedVersion;
        Name = name;
        Number = number;
        BankName = bankName;
        BankPhone = bankPhone;
        BankAddress = bankAddress;
        Enabled = enabled;
    }

    public Guid Id { get; set; }

    public long ExpectedVersion { get; set; }

    public string? Name { get; set; }

    public string? Number { get; set; }

    public string? BankName { get; set; }

    public string? BankPhone { get; set; }

    public string? BankAddress { get; set; }

    public bool? Enabled { get; set; }
}

public class UpdateAccountCommandHandler
{
    public static async Task<AccountCommandResult> Handle(UpdateAccountCommand request, IUnitOfWork unitOfWork, CancellationToken cancellationToken)
    {
        var accountAggregate = await unitOfWork.Accounts.LoadAsync(request.Id, cancellationToken) ?? throw new NotFoundException("Account not found");
        accountAggregate.Update(request.Name,
             request.Number,
             request.BankName,
             request.BankPhone,
             request.BankAddress,
             request.Enabled);
        await unitOfWork.Accounts.SaveAsync(accountAggregate, request.ExpectedVersion, cancellationToken);
        await unitOfWork.CommitAsync(cancellationToken);

        return new AccountCommandResult
        {
            AccountId = accountAggregate.Id,
            NewVersion = accountAggregate.Version
        };
    }
}
