using ExpenseTracker.Domain.Account.Events;
using ExpenseTracker.Application.Account;
using MediatR;

namespace ExpenseService.Application.Account.DeleteAccount;

public record DeleteAccountCommand(Guid Id, long Version) : IRequest;

public class DeleteAccountCommandHandler(IAccountRepository accountRepository) : IRequestHandler<DeleteAccountCommand>
{
    public async Task Handle(DeleteAccountCommand request, CancellationToken cancellationToken)
    {
        var @event = AccountDeletedEvent.Create(
             request.Id,
             DateTime.Now);

        accountRepository.Persist(request.Id, request.Version + 1, @event);
        await accountRepository.SaveChangesAsync(cancellationToken);
    }
}
