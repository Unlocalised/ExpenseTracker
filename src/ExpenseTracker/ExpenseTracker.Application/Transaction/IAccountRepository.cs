
using ExpenseTracker.Application.Common;
using ExpenseTracker.Domain.Transaction;

namespace ExpenseTracker.Application.Transaction;

public interface ITransactionRepository : IEventStoreRepository<TransactionAggregate>
{
}