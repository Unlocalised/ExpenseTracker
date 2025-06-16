using ExpenseService.Application.Common;
using ExpenseTracker.Application.Common.Exceptions;
using ExpenseTracker.Contracts.Account;

namespace ExpenseService.Application.Account.DeleteAccount;

public record DeleteAccountCommand(Guid Id, long ExpectedVersion);

public class DeleteAccountCommandHandler
{
    public static async Task Handle(DeleteAccountCommand request, IUnitOfWork unitOfWork, CancellationToken cancellationToken)
    {
        var accountAggregate = await unitOfWork.Accounts.LoadAsync(request.Id, cancellationToken) ?? throw new NotFoundException("Account not found");
        accountAggregate.Delete();
        await unitOfWork.Accounts.SaveAsync(accountAggregate, request.ExpectedVersion, cancellationToken);
        if (accountAggregate.DeletedAt.HasValue)
            await unitOfWork.PublishAsync(new AccountDeletedIntegrationEvent
            {
                Id = accountAggregate.Id,
                DeletedAt = accountAggregate.DeletedAt.Value
            });
        await unitOfWork.CommitAsync(cancellationToken);
    }
}
