using ExpenseTracker.Application.Common.Exceptions;
using ExpenseTracker.Application.Common;

namespace ExpenseService.Application.Account.DeleteAccount;

public record DeleteAccountCommand(Guid Id, long ExpectedVersion);

public class DeleteAccountCommandHandler
{
    public static async Task Handle(DeleteAccountCommand request, IUnitOfWork unitOfWork, CancellationToken cancellationToken)
    {
        var accountAggregate = await unitOfWork.Accounts.LoadAsync(request.Id, cancellationToken) ?? throw new NotFoundException("Account not found");
        accountAggregate.Delete();
        await unitOfWork.Accounts.SaveAsync(accountAggregate, request.ExpectedVersion, cancellationToken);
        await unitOfWork.CommitAsync(cancellationToken);
    }
}
