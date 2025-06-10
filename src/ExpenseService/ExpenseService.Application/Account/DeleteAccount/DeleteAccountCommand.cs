using ExpenseTracker.Application.Common.Exceptions;
using ExpenseTracker.Application.Common;
using MediatR;

namespace ExpenseService.Application.Account.DeleteAccount;

public record DeleteAccountCommand(Guid Id, long CurrentVersion) : IRequest;

public class DeleteAccountCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteAccountCommand>
{
    public async Task Handle(DeleteAccountCommand request, CancellationToken cancellationToken)
    {
        var accountAggregate = await unitOfWork.Accounts.LoadAsync(request.Id, cancellationToken) ?? throw new NotFoundException("Account not found");
        accountAggregate.Delete();
        await unitOfWork.Accounts.SaveAsync(accountAggregate, request.CurrentVersion, cancellationToken);
        await unitOfWork.CommitAsync(cancellationToken);
    }
}
