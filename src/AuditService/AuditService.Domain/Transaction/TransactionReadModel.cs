using ExpenseTracker.Contracts.Enums;

namespace AuditService.Domain.Transaction;

public class TransactionReadModel
{
    public Guid Id { get; set; }

    public decimal Amount { get; set; }

    public TransactionType TransactionType { get; set; }

    public Guid AccountId { get; set; }

    public DateTime? CreatedAt { get; set; }
}
