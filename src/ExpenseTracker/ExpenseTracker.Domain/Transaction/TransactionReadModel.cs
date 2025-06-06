using ExpenseTracker.Domain.Enums;

namespace ExpenseTracker.Domain.Transaction;

public class TransactionReadModel 
{
    public Guid Id { get; set; }

    public decimal Amount { get; set; }

    public TransactionType TransactionType { get; set; }

    public Guid AccountId { get; set; }

    public string? Number { get; set; }

    public DateTime? CreatedAt { get; set; }
}
