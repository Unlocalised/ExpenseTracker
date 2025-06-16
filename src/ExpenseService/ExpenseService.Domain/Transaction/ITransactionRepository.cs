
using ExpenseTracker.BuildingBlocks.Common;

namespace ExpenseService.Domain.Transaction;

public interface ITransactionRepository : IEventStoreRepository<TransactionAggregate>
{
}