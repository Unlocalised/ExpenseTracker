
using ExpenseTracker.BuildingBlocks.Common;

namespace ExpenseService.Domain.Account;

public interface IAccountRepository : IEventStoreRepository<AccountAggregate>
{
}