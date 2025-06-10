using ExpenseTracker.Application.Common;
using MediatR;
using ExpenseTracker.Application.Common.Exceptions;

namespace ExpenseService.Application.Account.UpdateAccount;

public record UpdateAccountCommand : IRequest
{
    public UpdateAccountCommand(Guid id, long currentVersion, string? name, string? number, string? bankName, string? bankPhone, string? bankAddress, bool? enabled)
    {
        Id = id;
        CurrentVersion = currentVersion;
        Name = name;
        Number = number;
        BankName = bankName;
        BankPhone = bankPhone;
        BankAddress = bankAddress;
        Enabled = enabled;
    }

    public Guid Id { get; set; }

    public long CurrentVersion { get; set; }

    public string? Name { get; set; }

    public string? Number { get; set; }

    public string? BankName { get; set; }

    public string? BankPhone { get; set; }

    public string? BankAddress { get; set; }

    public bool? Enabled { get; set; }
}

public class UpdateAccountCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<UpdateAccountCommand>
{
    public async Task Handle(UpdateAccountCommand request, CancellationToken cancellationToken)
    {
        var accountAggregate = await unitOfWork.Accounts.LoadAsync(request.Id, cancellationToken) ?? throw new NotFoundException("Account not found");
        accountAggregate.Update(request.Name,
             request.Number,
             request.BankName,
             request.BankPhone,
             request.BankAddress,
             request.Enabled);
        await unitOfWork.Accounts.SaveAsync(accountAggregate, request.CurrentVersion, cancellationToken);
        await unitOfWork.CommitAsync(cancellationToken);
    }
}
