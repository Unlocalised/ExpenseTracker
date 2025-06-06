using ExpenseTracker.Domain.Account.Events;
using ExpenseTracker.Application.Account;
using MediatR;

namespace ExpenseService.Application.Account.UpdateAccount;

public record UpdateAccountCommand : IRequest
{
    public Guid Id { get; set; }

    public long Version { get; set; }

    public string? Name { get; set; }

    public string? Number { get; set; }

    public string? BankName { get; set; }

    public string? BankPhone { get; set; }

    public string? BankAddress { get; set; }

    public bool? Enabled { get; set; }
}

public class UpdateAccountCommandHandler(IAccountRepository accountRepository) : IRequestHandler<UpdateAccountCommand>
{
    public async Task Handle(UpdateAccountCommand request, CancellationToken cancellationToken)
    {
        var @event = AccountUpdatedEvent.Create(
             request.Id,
             DateTime.Now,
             request.Name,
             request.Number,
             request.BankName,
             request.BankPhone,
             request.BankAddress,
             request.Enabled);

        accountRepository.Persist(request.Id, request.Version + 1, @event);
        await accountRepository.SaveChangesAsync(cancellationToken);
    }
}
