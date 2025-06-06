using ExpenseTracker.Domain.Common;

namespace ExpenseTracker.Domain.Account.Events;
public record AccountDeletedEvent : BaseEvent
{
    public Guid Id { get; set; }

    public DateTime DeletedAt { get; set; }

    public AccountDeletedEvent() { }

    public static AccountDeletedEvent Create(
        Guid id,
        DateTime deletedAt) => new()
        {
            Id = id,
            DeletedAt = deletedAt
        };
}