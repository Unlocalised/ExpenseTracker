
using ExpenseTracker.Application.Common;
using ExpenseTracker.Domain.Account;

namespace ExpenseTracker.Application.Account;

public interface IAccountRepository : IEventStoreRepository<AccountAggregate>
{
}